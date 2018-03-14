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

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The migration context</typeparam>
    public abstract class MigrationManager<TContext> : IMigrationManager<TContext> 
        where TContext : class, IMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="loggerFactory">An optional class logger factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MigrationManager(TContext context, IMigrationLoggerFactory loggerFactory = null)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = loggerFactory?.Get(GetType().FullName) ?? NullMigrationLogger.Default;
        }

        /// <summary>
        /// The migration context.
        /// </summary>
        protected TContext Context { get; }

        /// <summary>
        /// The class logger.
        /// </summary>
        protected IMigrationLogger Logger { get; }

        #region Implementation of IMigrationManager<out TContext>

        /// <inheritdoc />
        public async Task PrepareDatabaseAsync(CancellationToken ct)
        {
            Logger.LogDebug(null,
                "Preparing context '{contextName}' database to support migrations.", Context.NormalizedName);

            await Context.RunAsync(async () =>
            {
                if (await MigrationsHistoryExistAsync(ct).ConfigureAwait(false))
                {
                    Logger.LogInformation(null,
                        "Migrations history was detected in the database. No changes need to be done.");
                    return;
                }

                Logger.LogWarning(null, "Migrations history does not exist. Trying to create...");
                await CreateMigrationsHistoryAsync(ct).ConfigureAwait(false);
            }, true, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task AddMigrationAsync(string migrationId, string className, string description, CancellationToken ct)
        {
            if (migrationId == null)
                throw new ArgumentNullException(nameof(migrationId));
            if (className == null)
                throw new ArgumentNullException(nameof(className));
            if (string.IsNullOrWhiteSpace(migrationId))
                throw new ArgumentException("Value cannot be whitespace.", nameof(migrationId));
            if (string.IsNullOrWhiteSpace(className))
                throw new ArgumentException("Value cannot be whitespace.", nameof(className));

            migrationId = Context.Normalizer.Normalize(migrationId);
            className = Context.Normalizer.Normalize(className);
            Logger.LogDebug(null,
                "Adding '{migrationId}' to the history of '{contextName}' context.",
                migrationId, Context.NormalizedName);

            await Context.RunAsync(async () =>
            {
                var mostRecentMigrationId =
                    await GetMostRecentMigrationEntryIdAsync(Context.NormalizedName, ct).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(mostRecentMigrationId) ||
                    string.CompareOrdinal(migrationId, mostRecentMigrationId) > 0)
                {
                    await InsertMigrationEntryAsync(
                            Context.NormalizedName, migrationId, className, description, DateTimeOffset.Now, ct)
                        .ConfigureAwait(false);
                    return;
                }

                Logger.LogWarning(null,
                    "The history of '{contextName}' context has the migration '{mostRecentMigrationId}', which is considered more recent than '{migrationId}'.",
                    Context.NormalizedName, mostRecentMigrationId, migrationId);
                throw new InvalidOperationException(
                    $"The database already has the migration '{mostRecentMigrationId}' which is more recent than '{migrationId}'");
            }, true, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(CancellationToken ct)
        {
            var migrationIds =
                await Context.RunAsync(async () =>
                        await GetAllMigrationsAsync(Context.NormalizedName, ct).ConfigureAwait(false), true, ct)
                    .ConfigureAwait(false);

            Logger.LogDebug(null,
                "A total of {migrationCount} migrations have been found for the '{contextName}' context",
                migrationIds.Count.ToString(), Context.NormalizedName);
            return migrationIds;
        }

        /// <inheritdoc />
        public async Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct)
        {
            var migrationId =
                await Context.RunAsync(async () =>
                        await GetMostRecentMigrationEntryIdAsync(Context.NormalizedName, ct).ConfigureAwait(false), true, ct)
                    .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(migrationId))
            {
                Logger.LogDebug(null,
                    "No migrations have yet been applied to the '{contextName}' context.", Context.NormalizedName);
                return null;
            }

            Logger.LogDebug(null,
                "The migration '{migrationId}' is the most recent from the history of '{contextName}' context.",
                migrationId, Context.NormalizedName);
            return migrationId;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveMostRecentMigrationAsync(CancellationToken ct)
        {
            Logger.LogDebug(null,
                "Removing most recent migration from the history of '{contextName}' context.",
                Context.NormalizedName);

            var result = await Context.RunAsync(async () =>
            {
                var migrationId = await GetMostRecentMigrationEntryIdAsync(Context.NormalizedName, ct)
                    .ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(migrationId))
                {
                    Logger.LogWarning(null,
                        "No migrations were found in history of '{contextName}' context. No changes will be made.",
                        Context.NormalizedName);
                    return false;
                }

                Logger.LogDebug(null,
                    "Removing migration '{migradionId}' from the history of '{contextName}' context",
                    migrationId, Context.NormalizedName);
                await DeleteMigrationEntryByIdAsync(Context.NormalizedName, migrationId, ct).ConfigureAwait(false);
                return true;
            }, true, ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        /// <summary>
        /// Checks it the migrations history exists in the database.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        protected abstract Task<bool> MigrationsHistoryExistAsync(CancellationToken ct);

        /// <summary>
        /// Creates the migrations history in the database.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        protected abstract Task CreateMigrationsHistoryAsync(CancellationToken ct);

        /// <summary>
        /// Inserts a migration entry into the table.
        /// </summary>
        /// <param name="contextName">The migration context name</param>
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="className">The class responsible for this migration</param>
        /// <param name="description">The migration description</param>
        /// <param name="appliedOn">The date the migration was applied</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        protected abstract Task InsertMigrationEntryAsync(
            string contextName, string migrationId, string className, string description, DateTimeOffset appliedOn, CancellationToken ct);

        /// <summary>
        /// Returns a collection of all migrations ids currently applied.
        /// </summary>
        /// <param name="contextName">The migration context name</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        protected abstract Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(string contextName, CancellationToken ct);

        /// <summary>
        /// Gets the identifier of the most recently applied migration.
        /// </summary>
        /// <param name="contextName">The migration context name</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        protected abstract Task<string> GetMostRecentMigrationEntryIdAsync(string contextName, CancellationToken ct);

        /// <summary>
        /// Deletes a migration by its identifier.
        /// </summary>
        /// <param name="contextName">The migration context name</param>
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        protected abstract Task DeleteMigrationEntryByIdAsync(string contextName, string migrationId, CancellationToken ct);
    }
}
