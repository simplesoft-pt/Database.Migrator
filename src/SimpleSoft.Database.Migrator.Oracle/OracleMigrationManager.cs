using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Oracle
{
    /// <summary>
    /// Manages migration states for Oracle
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class OracleMigrationManager<TContext> : RelationalMigrationManager<TContext>, IOracleMigrationManager<TContext>
        where TContext : IRelationalMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="normalizer">The naming normalizer</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OracleMigrationManager(TContext context, INamingNormalizer normalizer,
            ILogger<OracleMigrationManager<TContext>> logger)
            : base(context, normalizer, logger)
        {

        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="normalizer">The naming normalizer</param>
        /// <param name="logger">The logger</param>
        /// <param name="contextName">The context name</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public OracleMigrationManager(TContext context, INamingNormalizer normalizer,
            ILogger<OracleMigrationManager<TContext>> logger, string contextName)
            : base(context, normalizer, logger, contextName)
        {

        }

        #region Overrides of MigrationManager<TContext>

        /// <inheritdoc />
        protected override async Task<bool> MigrationsHistoryExistAsync(CancellationToken ct)
        {
            var tableCount = await Context.QuerySingleAsync<int>(@"
SELECT COUNT(*)
FROM SYS.ALL_OBJECTS
WHERE
  OBJECT_TYPE = 'TABLE'
  AND OWNER = SYS_CONTEXT ('USERENV', 'SESSION_USER')
  AND OBJECT_NAME = 'DB_MIGRATOR_HISTORY'").ConfigureAwait(false);

            return tableCount > 0;
        }

        /// <inheritdoc />
        protected override Task CreateMigrationsHistoryAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task InsertMigrationEntryAsync(string contextName, string migrationId, string className, DateTimeOffset appliedOn,
            CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(string contextName, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task<string> GetMostRecentMigrationEntryIdAsync(string contextName, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task DeleteMigrationEntryByIdAsync(string contextName, string migrationId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}