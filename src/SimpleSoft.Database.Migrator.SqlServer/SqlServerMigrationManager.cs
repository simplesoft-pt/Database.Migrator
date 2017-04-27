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
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class SqlServerMigrationManager<TContext>: RelationalMigrationManager<TContext>
        where TContext : IRelationalMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlServerMigrationManager(TContext context, ILogger<RelationalMigrationManager<TContext>> logger) 
            : base(context, logger)
        {

        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="logger">The logger</param>
        /// <param name="contextName">The context name</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SqlServerMigrationManager(TContext context, ILogger<MigrationManager<TContext>> logger, string contextName) 
            : base(context, logger, contextName)
        {

        }

        #region Overrides of MigrationManager<TContext>

        /// <inheritdoc />
        protected override async Task<bool> MigrationsHistoryExistAsync(CancellationToken ct)
        {
            var tableId = await Context.QuerySingleAsync<long?>(
                    "SELECT OBJECT_ID(@TableName, 'U') as TableId", new
                    {
                        TableName = MigrationsHistoryTableName
                    })
                .ConfigureAwait(false);
            return tableId.HasValue;
        }

        /// <inheritdoc />
        protected override async Task CreateMigrationsHistoryAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync($@"
CREATE TABLE {MigrationsHistoryTableName}
(
    ContextName NVARCHAR(256) NOT NULL,
    MigrationId NVARCHAR(128) NOT NULL,
    ClassName NVARCHAR(512) NOT NULL,
    AppliedOn DATETIME2 NOT NULL,
    PRIMARY KEY (ContextName, MigrationId)
)")
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task InsertMigrationEntryAsync(string contextName, string migrationId, string className, DateTimeOffset appliedOn, CancellationToken ct)
        {
            await Context.ExecuteAsync($@"
INSERT INTO {MigrationsHistoryTableName}(ContextName, MigrationId, ClassName, AppliedOn) 
VALUES (@ContextName, @MigrationId, @ClassName, @AppliedOn)", new
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
            var result = await Context.Query<string>($@"
SELECT 
    MigrationId
FROM {MigrationsHistoryTableName}
WHERE
    ContextName = @ContextName
ORDER BY 
    ContextName DESC, MigrationId DESC", new
                {
                    ContextName = contextName
                })
                .ConfigureAwait(false);

            return result as IReadOnlyCollection<string> ?? result.ToList();
        }

        /// <inheritdoc />
        protected override async Task<string> GetMostRecentMigrationEntryIdAsync(string contextName, CancellationToken ct)
        {
            var migrationId = await Context.QuerySingleOrDefaultAsync<string>($@"
SELECT 
    TOP(1) MigrationId 
FROM {MigrationsHistoryTableName} 
WHERE
    ContextName = @ContextName
ORDER BY 
    ContextName DESC, MigrationId DESC", new
                {
                    ContextName = contextName
                })
                .ConfigureAwait(false);

            return migrationId;
        }

        /// <inheritdoc />
        protected override async Task DeleteMigrationEntryByIdAsync(string contextName, string migrationId, CancellationToken ct)
        {
            await Context.ExecuteAsync($@"
DELETE FROM {MigrationsHistoryTableName} 
WHERE 
    ContextName = @ContextName
    AND MigrationId = @MigrationId", new
                {
                    ContextName = contextName,
                    MigrationId = migrationId
                })
                .ConfigureAwait(false);
        }

        #endregion
    }
}
