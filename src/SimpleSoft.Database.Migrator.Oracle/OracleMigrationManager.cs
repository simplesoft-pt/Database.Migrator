using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
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
        protected override async Task CreateMigrationsHistoryAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
CREATE TABLE DB_MIGRATOR_HISTORY(
    CONTEXT_NAME VARCHAR(256) NOT NULL,
    MIGRATION_ID VARCHAR(128) NOT NULL,
    CLASS_NAME VARCHAR(512) NOT NULL,
    APPLIED_ON TIMESTAMP WITH TIME ZONE NOT NULL,
    PRIMARY KEY (CONTEXT_NAME, MIGRATION_ID)
)")
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task InsertMigrationEntryAsync(string contextName, string migrationId, string className, DateTimeOffset appliedOn, CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
INSERT INTO DB_MIGRATOR_HISTORY(CONTEXT_NAME, MIGRATION_ID, CLASS_NAME, APPLIED_ON)
VALUES (:ContextName, :MigrationId, :ClassName, :AppliedOn)", new
                {
                    ContextName = contextName,
                    MigrationId = migrationId,
                    ClassName = className,
                    AppliedOn = appliedOn
                })
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(string contextName, CancellationToken ct)
        {
            var result = await Context.Query<string>(@"
SELECT 
  MIGRATION_ID
FROM DB_MIGRATOR_HISTORY
WHERE
  CONTEXT_NAME = :ContextName
ORDER BY
  CONTEXT_NAME ASC, MIGRATION_ID DESC", new
                {
                    ContextName = contextName
                })
                .ConfigureAwait(false);

            return result as IReadOnlyCollection<string> ?? result.ToList();
        }

        /// <inheritdoc />
        protected override async Task<string> GetMostRecentMigrationEntryIdAsync(string contextName, CancellationToken ct)
        {
            var migrationId = await Context.QuerySingleOrDefaultAsync<string>(@"
SELECT MIGRATION_ID
FROM (
  SELECT 
    MIGRATION_ID
  FROM DB_MIGRATOR_HISTORY
  WHERE
    CONTEXT_NAME = :ContextName
  ORDER BY
    CONTEXT_NAME ASC, MIGRATION_ID DESC
)
WHERE
  ROWNUM < 2", new
                {
                    ContextName = contextName
                })
                .ConfigureAwait(false);

            return migrationId;
        }

        /// <inheritdoc />
        protected override async Task DeleteMigrationEntryByIdAsync(string contextName, string migrationId, CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
DELETE FROM DB_MIGRATOR_HISTORY 
WHERE 
    CONTEXT_NAME = :ContextName
    AND MIGRATION_ID = :MigrationId", new
                {
                    ContextName = contextName,
                    MigrationId = migrationId
                })
                .ConfigureAwait(false);
        }

        #endregion
    }
}