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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="loggerFactory">The logger factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OracleMigrationManager(TContext context, INamingNormalizer<TContext> normalizer,
            IMigrationLoggerFactory loggerFactory)
            : base(context, normalizer, loggerFactory)
        {

        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="normalizer">The naming normalizer</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="contextName">The context name</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public OracleMigrationManager(TContext context, INamingNormalizer<TContext> normalizer,
            IMigrationLoggerFactory loggerFactory, string contextName)
            : base(context, normalizer, loggerFactory, contextName)
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
  AND OBJECT_NAME = 'DB_MIGRATOR_HISTORY'", ct: ct).ConfigureAwait(false);

            return tableCount > 0;
        }

        /// <inheritdoc />
        protected override async Task CreateMigrationsHistoryAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
CREATE TABLE DB_MIGRATOR_HISTORY(
    CONTEXT_NAME VARCHAR(256) NOT NULL,
    MIGRATION_ID VARCHAR(256) NOT NULL,
    CLASS_NAME VARCHAR(1024) NOT NULL,
    DESCRIPTION VARCHAR(4000) NULL,
    APPLIED_ON TIMESTAMP WITH TIME ZONE NOT NULL,
    PRIMARY KEY (CONTEXT_NAME, MIGRATION_ID)
)", ct: ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task InsertMigrationEntryAsync(string contextName, string migrationId, 
            string className, string description, DateTimeOffset appliedOn, CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
INSERT INTO DB_MIGRATOR_HISTORY(CONTEXT_NAME, MIGRATION_ID, CLASS_NAME, DESCRIPTION, APPLIED_ON)
VALUES (:ContextName, :MigrationId, :ClassName, :Description, TO_TIMESTAMP_TZ(:AppliedOn,'YYYY-MM-DD""T""HH24:MI:SS.FF7TZH:TZM'))", new
                {
                    ContextName = contextName,
                    MigrationId = migrationId,
                    ClassName = className,
                    Description = description,
                    AppliedOn = appliedOn.ToString("O")
            }, ct: ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(string contextName, CancellationToken ct)
        {
            var result = await Context.QueryAsync<string>(@"
SELECT 
  MIGRATION_ID
FROM DB_MIGRATOR_HISTORY
WHERE
  CONTEXT_NAME = :ContextName
ORDER BY
  CONTEXT_NAME ASC, MIGRATION_ID DESC", new
                {
                    ContextName = contextName
                }, ct: ct)
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
                }, ct: ct)
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
                }, ct: ct)
                .ConfigureAwait(false);
        }

        #endregion
    }
}