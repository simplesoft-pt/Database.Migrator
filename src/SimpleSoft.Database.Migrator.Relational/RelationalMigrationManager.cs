using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public abstract class RelationalMigrationManager<TContext> : IMigrationManager<TContext> 
        where TContext : IRelationalMigrationContext
    {
        private readonly string _contextTypeName;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RelationalMigrationManager(TContext context, 
            ILogger<RelationalMigrationManager<TContext>> logger)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _contextTypeName = typeof(TContext).Name;

            Context = context;
            Logger = logger;
        }

        /// <summary>
        /// The migrations history table name
        /// </summary>
        public string MigrationsHistoryTableName { get; set; } = "__DbMigratorHistory";

        /// <summary>
        /// The logger used by this instance
        /// </summary>
        protected ILogger<RelationalMigrationManager<TContext>> Logger { get; }

        #region Implementation of IMigrationManager<out TOptions>

        /// <inheritdoc />
        public TContext Context { get; }

        /// <inheritdoc />
        public virtual async Task PrepareDatabaseAsync(CancellationToken ct)
        {
            Logger.LogDebug(
                "Preparing context '{contextName}' database for migrations.", _contextTypeName);

            await Context.ExecuteAsync(async (ctx, c) =>
                {
                    if (await MigrationsTableExistAsync(c).ConfigureAwait(false))
                    {
                        Logger.LogInformation(
                            "Migrations history table was detected in the database. Nothing needs to be done.");
                        return;
                    }

                    Logger.LogWarning("Migrations history table does not exist. Trying to create the table...");
                    await CreateMigrationsTableAsync(c).ConfigureAwait(false);
                }, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task AddMigrationAsync(string migrationId, string className, CancellationToken ct)
        {
            Logger.LogDebug(
                "Adding '{migrationId}' to the history table of '{contextName}' context.",
                migrationId, _contextTypeName);

            await Context.ExecuteAsync(async (ctx, c) =>
                {
                    await InsertMigrationEntryAsync(
                            migrationId, className, DateTimeOffset.Now, c)
                        .ConfigureAwait(false);
                }, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct)
        {
            var migrationId =
                await Context.ExecuteAsync(async (ctx, c) =>
                        await GetMostRecentMigrationEntryIdAsync(c).ConfigureAwait(false), ct)
                    .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(migrationId))
            {
                Logger.LogDebug(
                    "No migrations have yet been applied to the '{contextName}' context.", _contextTypeName);
                return null;
            }

            Logger.LogDebug(
                "The migration '{migrationId}' is the most recent from the history table of '{contextName}' context.",
                migrationId, _contextTypeName);
            return migrationId;
        }

        /// <inheritdoc />
        public virtual async Task<bool> RemoveMostRecentMigrationAsync(CancellationToken ct)
        {
            Logger.LogDebug(
                "Removing most recent migration from the history table of '{contextName}' context.",
                _contextTypeName);

            var result = await Context.ExecuteAsync(async (ctx, c) =>
            {
                var migrationId = await GetMostRecentMigrationEntryIdAsync(c);
                if (string.IsNullOrWhiteSpace(migrationId))
                {
                    Logger.LogWarning(
                        "No migrations were found in history table of '{contextName}' context. No changes will be made.",
                        _contextTypeName);
                    return false;
                }

                Logger.LogDebug(
                    "Removing migration '{migradionId}' from the history of '{contextName}' context",
                    migrationId, _contextTypeName);
                await DeleteMigrationEntryByIdAsync(migrationId, c);
                return true;
            }, ct);

            return result;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Checks it the migrations history table exists in the database.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        protected abstract Task<bool> MigrationsTableExistAsync(CancellationToken ct);

        /// <summary>
        /// Creates the migrations history table in the database.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        protected abstract Task CreateMigrationsTableAsync(CancellationToken ct);

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

        #endregion
    }
}
