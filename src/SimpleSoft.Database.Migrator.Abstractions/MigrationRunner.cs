#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 João Simões
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Represents a configured migrator host
    /// </summary>
    public class MigrationRunner<TContext> : IMigrationRunner<TContext> where TContext : IMigrationContext
    {
        private readonly SortedList<string, MigrationMetadata<TContext>> _migrations;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="namingNormalizer">The naming normalizer</param>
        /// <param name="migrations">The migrations found</param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MigrationRunner(
            IServiceProvider serviceProvider, INamingNormalizer<TContext> namingNormalizer, 
            IEnumerable<MigrationMetadata<TContext>> migrations, ILogger<MigrationRunner<TContext>> logger)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (namingNormalizer == null)
                throw new ArgumentNullException(nameof(namingNormalizer));
            if (migrations == null)
                throw new ArgumentNullException(nameof(migrations));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Logger = logger;
            ContextName = namingNormalizer.Normalize(typeof(TContext).Name);
            NamingNormalizer = namingNormalizer;
            ServiceProvider = serviceProvider;
            ServiceScopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();

            _migrations = new SortedList<string, MigrationMetadata<TContext>>();
            foreach (var migration in migrations)
            {
                var name = namingNormalizer.Normalize(migration.Id);
                if (_migrations.ContainsKey(name))
                    throw new ArgumentException(
                        $"Collection contains duplicated migrations with name '{name}' for context '{ContextName}'",
                        nameof(migrations));

                _migrations.Add(migration.Id, new MigrationMetadata<TContext>(
                    name, namingNormalizer.Normalize(migration.ClassName), migration.Type));
            }
        }

        #region Implementation of IMigratorHost<TContext>

        /// <summary>
        /// The host logger
        /// </summary>
        protected ILogger<MigrationRunner<TContext>> Logger { get; }

        /// <summary>
        /// The service provider
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// The service scope factory
        /// </summary>
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// The naming normalized
        /// </summary>
        protected INamingNormalizer<TContext> NamingNormalizer { get; }

        /// <summary>
        /// The context name
        /// </summary>
        protected string ContextName { get; }

        /// <inheritdoc />
        public IEnumerable<MigrationMetadata<TContext>> MigrationMetadatas => _migrations.Values;

        /// <inheritdoc />
        public async Task ApplyMigrationsAsync(CancellationToken ct)
        {
            if (_migrations.Count == 0)
            {
                Logger.LogWarning(
                    "The migration collection for '{contextName}' context is empty. Nothing to be done.",
                    ContextName);
                return;
            }

            var lastMigrationId = _migrations.Keys[_migrations.Count - 1];
            await ApplyMigrationsAsync(lastMigrationId, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task ApplyMigrationsAsync(string migrationId, CancellationToken ct)
        {
            if (migrationId == null)
                throw new ArgumentNullException(nameof(migrationId));
            if (string.IsNullOrWhiteSpace(migrationId))
                throw new ArgumentException("Value cannot be whitespace.", nameof(migrationId));

            using (Logger.BeginScope("Context:{contextName} TargetMigrationId:{migrationId}", ContextName, migrationId))
            {
                migrationId = NamingNormalizer.Normalize(migrationId);
                Logger.LogInformation("About to apply migration", migrationId);

                if (!_migrations.ContainsKey(migrationId))
                    throw new InvalidOperationException(
                        $"There is no matching migration with the identifier '{migrationId}'");

                if (_migrations.Count == 0)
                {
                    Logger.LogWarning("The migration collection is empty. Nothing to be done.");
                    return;
                }

                await UsingManagerAsync(async manager =>
                {
                    await manager.PrepareDatabaseAsync(ct).ConfigureAwait(false);

                    var migrationStartIdx =
                        await CalculateMigrationStartIndexAsync(migrationId, manager, ct).ConfigureAwait(false);
                    if (migrationStartIdx == -1)
                    {
                        Logger.LogInformation("Migration is already applied. Nothing to be done.", migrationId);
                        return;
                    }

                    Logger.LogDebug(
                        "Migrations starting at position {migrationStartIdx} for a total of {migrationCount} migrations",
                        migrationStartIdx, _migrations.Count);
                    for (; migrationStartIdx < _migrations.Count; migrationStartIdx++)
                    {
                        var migrationMeta = _migrations.Values[migrationStartIdx];
                        using (Logger.BeginScope("CurrentMigrationId:{currentmigrationId}", migrationMeta.Id))
                        {
                            await UsingMigrationAsync(migrationMeta.Type, async migration =>
                            {
                                Logger.LogDebug(
                                    "Applying migration [RunInsideScope: {runInsideScope}]",
                                    migration.RunInTransaction);

                                await migration.Context.RunAsync(async () =>
                                {
                                    await migration.ApplyAsync(ct).ConfigureAwait(false);
                                }, migration.RunInTransaction, ct).ConfigureAwait(false);

                            }).ConfigureAwait(false);
                            
                            await manager.AddMigrationAsync(migrationMeta.Id, migrationMeta.ClassName, ct)
                                .ConfigureAwait(false);

                            Logger.LogInformation("Migration applied to the database");
                        }
                    }

                }).ConfigureAwait(false);
            }
        }

        #endregion

        /// <summary>
        /// Gets a new <see cref="IMigrationManager{TContext}"/> from the 
        /// service scope factory
        /// </summary>
        /// <param name="handler">The handler to receive the manager</param>
        /// <returns>A task to be awaited</returns>
        protected async Task UsingManagerAsync(Func<IMigrationManager<TContext>, Task> handler)
        {
            using (var managerScope = ServiceScopeFactory.CreateScope())
            {
                Logger.LogDebug("Resolving migration manager from service collection");
                var manager =
                    managerScope.ServiceProvider.GetRequiredService<IMigrationManager<TContext>>();

                await handler(manager).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets a new <see cref="IMigration{TContext}"/> from the service
        /// scope factory for the given type
        /// </summary>
        /// <param name="migrationType">The migration type to resolve</param>
        /// <param name="handler">The handler to receive the migration</param>
        /// <returns>A task to be awaited</returns>
        protected async Task UsingMigrationAsync(Type migrationType, Func<IMigration<TContext>, Task> handler)
        {
            using (var migrationScope = ServiceScopeFactory.CreateScope())
            {
                Logger.LogDebug(
                    "Resolving migration of type '{migrationType}' from service collection",
                    migrationType);
                var migration = (IMigration<TContext>)
                    migrationScope.ServiceProvider.GetRequiredService(migrationType);

                await handler(migration).ConfigureAwait(false);
            }
        }

        private async Task<int> CalculateMigrationStartIndexAsync(
            string migrationId, IMigrationManager<TContext> manager, CancellationToken ct)
        {
            var dbMigrations = await manager.GetAllMigrationsAsync(ct).ConfigureAwait(false);
            if (dbMigrations.Count <= 0)
                return 0;

            var sortedDbMigrations = new SortedList<string, string>(dbMigrations.Count);
            foreach (var dbMigration in dbMigrations)
                sortedDbMigrations.Add(dbMigration, dbMigration);

            FailIfAppliedMigrationsMismatchInMemory(sortedDbMigrations.Keys);

            return sortedDbMigrations.ContainsKey(migrationId)
                ? -1
                : sortedDbMigrations.Count;
        }

        private void FailIfAppliedMigrationsMismatchInMemory(IList<string> sortedDbMigrations)
        {
            if(sortedDbMigrations.Count == 0) return;

            if (_migrations.Count < sortedDbMigrations.Count)
                throw new InvalidOperationException(
                    "Registered migrations are fewer than the ones currently applied to the database");

            for (var i = 0; i < Math.Min(sortedDbMigrations.Count, _migrations.Keys.Count); i++)
            {
                var dbMigrationId = sortedDbMigrations[i];
                var memoryMigrationId = _migrations.Keys[i];

                if (string.CompareOrdinal(dbMigrationId, memoryMigrationId) != 0)
                {
                    throw new InvalidOperationException(
                        $"Mismatch between database migrations and registered ones. Expecting {dbMigrationId} but found {memoryMigrationId} for the same order");
                }
            }
        }
    }
}
