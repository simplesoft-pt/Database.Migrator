using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class RelationalMigrationManager<TContext> : IMigrationManager<TContext> 
        where TContext : IRelationalMigrationContext
    {
        private readonly ILogger<RelationalMigrationManager<TContext>> _logger;
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

            _logger = logger;
            _contextTypeName = typeof(TContext).Name;

            Context = context;
        }

        #region Implementation of IMigrationManager<out TOptions>

        /// <inheritdoc />
        public TContext Context { get; }

        /// <inheritdoc />
        public virtual async Task PrepareDatabaseAsync(CancellationToken ct)
        {
            _logger.LogDebug(
                "Preparing context '{contextName}' database for migrations", _contextTypeName);

            await Context.ExecuteAsync(async (ctx, c) =>
            {
                try
                {
                    await ctx.Connection.QuerySingleAsync<long>(
                        "SELECT COUNT(*) FROM MIGRATOR_HISTORY",
                        transaction: ctx.Transaction, commandTimeout: ctx.Connection.ConnectionTimeout);

                    _logger.LogDebug(
                        "Migration history table was detected in the database. Nothing needs to be done.");
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(0, e,
                        "Failed to read the migration history table. Trying to create the table...");
                }


            }, ct);

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> AddMigrationAsync(string migrationId, string className, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> RemoveMigrationAsync(string migrationId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
