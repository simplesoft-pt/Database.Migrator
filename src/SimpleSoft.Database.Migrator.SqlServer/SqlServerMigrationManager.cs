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
        /// <inheritdoc />
        public SqlServerMigrationManager(TContext context, ILogger<RelationalMigrationManager<TContext>> logger) 
            : base(context, logger)
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
	MigrationId NVARCHAR(150) PRIMARY KEY NOT NULL,
	ClassName NVARCHAR(500) NOT NULL,
	AppliedOn DATETIME2 NOT NULL
)")
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task InsertMigrationEntryAsync(string migrationId, string className, DateTimeOffset appliedOn, CancellationToken ct)
        {
            await Context.ExecuteAsync($@"
INSERT INTO {MigrationsHistoryTableName}(MigrationId, ClassName, AppliedOn) 
VALUES (@MigrationId, @ClassName, @AppliedOn)", new
            {
                MigrationId = migrationId,
                ClassName = className,
                AppliedOn = appliedOn
            });
        }

        /// <inheritdoc />
        protected override async Task<string> GetMostRecentMigrationEntryIdAsync(CancellationToken ct)
        {
            var migrationId = await Context.QuerySingleAsync<string>($@"
SELECT 
    TOP(1) MigrationId 
FROM {MigrationsHistoryTableName} 
ORDER BY 
    MigrationId DESC")
                .ConfigureAwait(false);

            return migrationId;
        }

        /// <inheritdoc />
        protected override async Task DeleteMigrationEntryByIdAsync(string migrationId, CancellationToken ct)
        {
            await Context.ExecuteAsync($@"
DELETE FROM {MigrationsHistoryTableName} 
WHERE 
    MigrationId = @MigrationId", new
                {
                    MigrationId = migrationId
                })
                .ConfigureAwait(false);
        }

        #endregion
    }
}
