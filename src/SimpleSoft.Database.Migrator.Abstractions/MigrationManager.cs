using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The migration context</typeparam>
    public abstract class MigrationManager<TContext> : IMigrationManager<TContext> 
        where TContext : IMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="logger">The class logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MigrationManager(TContext context, ILogger<MigrationManager<TContext>> logger)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ContextTypeName = typeof(TContext).Name;
            Context = context;
            Logger = logger;
        }

        /// <summary>
        /// The context type name
        /// </summary>
        protected string ContextTypeName { get; }

        /// <summary>
        /// The class logger
        /// </summary>
        protected ILogger<MigrationManager<TContext>> Logger { get; }

        #region Implementation of IMigrationManager<out TContext>

        /// <inheritdoc />
        public TContext Context { get; }

        /// <inheritdoc />
        public async Task PrepareDatabaseAsync(CancellationToken ct)
        {
            Logger.LogDebug(
                "Preparing context '{contextName}' database to support migrations.", ContextTypeName);

            await Context.ExecuteAsync(async (ctx, c) =>
                {
                    if (await MigrationsHistoryExistAsync(c).ConfigureAwait(false))
                    {
                        Logger.LogInformation(
                            "Migrations history was detected in the database. No changes need to be done.");
                        return;
                    }

                    Logger.LogWarning("Migrations history does not exist. Trying to create...");
                    await CreateMigrationsHistoryAsync(c).ConfigureAwait(false);
                }, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task AddMigrationAsync(string migrationId, string className, CancellationToken ct)
        {
            if (migrationId == null)
                throw new ArgumentNullException(nameof(migrationId));
            if (className == null)
                throw new ArgumentNullException(nameof(className));
            if (string.IsNullOrWhiteSpace(migrationId))
                throw new ArgumentException("Value cannot be whitespace.", nameof(migrationId));
            if (string.IsNullOrWhiteSpace(className))
                throw new ArgumentException("Value cannot be whitespace.", nameof(className));

            Logger.LogDebug(
                "Adding '{migrationId}' to the history of '{contextName}' context.",
                migrationId, ContextTypeName);

            await Context.ExecuteAsync(async (ctx, c) =>
                {
                    var mostRecentMigrationId =
                        await GetMostRecentMigrationEntryIdAsync(c).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(mostRecentMigrationId) ||
                        string.Compare(migrationId, mostRecentMigrationId, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        await InsertMigrationEntryAsync(migrationId, className, DateTimeOffset.Now, c)
                            .ConfigureAwait(false);
                        return;
                    }

                    Logger.LogWarning(
                        "The history of '{contextName}' context has the migration '{mostRecentMigrationId}', which is considered more recent than '{migrationId}'.",
                        ContextTypeName, mostRecentMigrationId, migrationId);
#warning Create custom exception
                    throw new Exception("A more recent migration already exists in the database");
                }, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct)
        {
            var migrationId =
                await Context.ExecuteAsync(async (ctx, c) =>
                        await GetMostRecentMigrationEntryIdAsync(c).ConfigureAwait(false), ct)
                    .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(migrationId))
            {
                Logger.LogDebug(
                    "No migrations have yet been applied to the '{contextName}' context.", ContextTypeName);
                return null;
            }

            Logger.LogDebug(
                "The migration '{migrationId}' is the most recent from the history of '{contextName}' context.",
                migrationId, ContextTypeName);
            return migrationId;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveMostRecentMigrationAsync(CancellationToken ct)
        {
            Logger.LogDebug(
                "Removing most recent migration from the history of '{contextName}' context.",
                ContextTypeName);

            var result = await Context.ExecuteAsync(async (ctx, c) =>
                {
                    var migrationId = await GetMostRecentMigrationEntryIdAsync(c).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(migrationId))
                    {
                        Logger.LogWarning(
                            "No migrations were found in history of '{contextName}' context. No changes will be made.",
                            ContextTypeName);
                        return false;
                    }

                    Logger.LogDebug(
                        "Removing migration '{migradionId}' from the history of '{contextName}' context",
                        migrationId, ContextTypeName);
                    await DeleteMigrationEntryByIdAsync(migrationId, c).ConfigureAwait(false);
                    return true;
                }, ct)
                .ConfigureAwait(false);

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
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="className">The class responsible for this migration</param>
        /// <param name="appliedOn">The date the migration was applied</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        protected abstract Task InsertMigrationEntryAsync(
            string migrationId, string className, DateTimeOffset appliedOn, CancellationToken ct);

        /// <summary>
        /// Gets the identifier of the most recently applied migration.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        protected abstract Task<string> GetMostRecentMigrationEntryIdAsync(CancellationToken ct);

        /// <summary>
        /// Deletes a migration by its identifier.
        /// </summary>
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        protected abstract Task DeleteMigrationEntryByIdAsync(string migrationId, CancellationToken ct);
    }
}
