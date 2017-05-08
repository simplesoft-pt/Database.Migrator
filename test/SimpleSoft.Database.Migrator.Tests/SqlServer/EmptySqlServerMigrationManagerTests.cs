using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class EmptySqlServerMigrationManagerTests: IClassFixture<EmptySqlDatabaseFixture>
    {
        private readonly EmptySqlDatabaseFixture _fixture;

        public EmptySqlServerMigrationManagerTests(EmptySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenAnEmptyDatabaseWhenPreparingForMigrationsThenATableMustBeCreated()
        {
            var ct = CancellationToken.None;

            var manager = new SqlServerMigrationManager<MigratorTestContext>(
                _fixture.Context, new DefaultNamingNormalizer(), LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());

            await manager.PrepareDatabaseAsync(ct);

            var tableId = await manager.Context.RunAsync(async () =>
                await manager.Context.QuerySingleAsync<long>(@"
select count(*) as Total
from (
	select 
        MigrationId, ClassName, AppliedOn
	from " + manager.MigrationsHistoryTableName + @"
) V"), ct);
            Assert.NotNull(tableId);
            Assert.Equal(0L, tableId);
        }
    }
}