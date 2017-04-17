﻿using System.Threading.Tasks;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class SqlServerMigrationManagerTests
    {
        [Fact]
        public async Task GivenAnEmptyDatabaseWhenPreparingForMigrationsThenATableMustBeCreated()
        {
            await SqlServerTestHelpers.MigratorTestContextAsync(async (ctx, ct) =>
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(
                    ctx, LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());

                var tableId = await manager.Context.RunAsync(async () =>
                    await manager.Context.QuerySingleAsync<long?>(
                        "SELECT OBJECT_ID(@TableName, 'U') as TableId", new
                        {
                            TableName = manager.MigrationsHistoryTableName
                        }), ct);
                Assert.Null(tableId);

                await manager.PrepareDatabaseAsync(ct);
                
                tableId = await manager.Context.RunAsync(async () =>
                    await manager.Context.QuerySingleAsync<long?>(
                        "SELECT OBJECT_ID(@TableName, 'U') as TableId", new
                        {
                            TableName = manager.MigrationsHistoryTableName
                        }), ct);
                Assert.NotNull(tableId);
            });
        }
    }
}
