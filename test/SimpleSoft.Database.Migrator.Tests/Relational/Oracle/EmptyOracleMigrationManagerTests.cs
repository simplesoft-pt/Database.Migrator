using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Relational.Oracle
{
    public class EmptyOracleMigrationManagerTests : IClassFixture<EmptyDatabaseFixture>
    {
        private readonly EmptyDatabaseFixture _fixture;

        public EmptyOracleMigrationManagerTests(EmptyDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenAnEmptyDatabaseWhenPreparingForMigrationsThenATableMustBeCreated()
        {
            var ct = CancellationToken.None;

            var manager = new OracleMigrationManager<MigratorTestContext>(
                _fixture.Context, new DefaultNamingNormalizer<MigratorTestContext>(), 
                LoggingManager.CreateTestLogger<OracleMigrationManager<MigratorTestContext>>());

            await manager.PrepareDatabaseAsync(ct);

            var tableId = await manager.Context.RunAsync(async () =>
                await manager.Context.QuerySingleAsync<long>(@"
SELECT COUNT(*) TOTAL
FROM(
  SELECT 
    --  making sure all columns exist
    CONTEXT_NAME, MIGRATION_ID, CLASS_NAME, DESCRIPTION, APPLIED_ON
  FROM MIGRATORTESTEMPTY.DB_MIGRATOR_HISTORY
)"), true, ct);
            Assert.NotNull(tableId);
            Assert.Equal(0L, tableId);
        }
    }
}