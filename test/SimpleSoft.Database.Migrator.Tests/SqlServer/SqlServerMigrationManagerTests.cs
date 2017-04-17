using System.Threading.Tasks;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class SqlServerMigrationManagerTests
    {
        [Fact]
        public async Task GivenAnEmptyDatabaseWhenUsingTheMigratorThenOneMustBeCreated()
        {
            await SqlServerTestHelpers.MigratorTestContextAsync(async (ctx, ct) =>
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(
                    ctx, LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());

                await manager.PrepareDatabaseAsync(ct);
            });
        }
    }
}
