using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer
{
    public class EmptySqlServerMigrationManagerTests: IClassFixture<EmptyDatabaseFixture>
    {
        private readonly EmptyDatabaseFixture _fixture;

        public EmptySqlServerMigrationManagerTests(EmptyDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenAnEmptyDatabaseWhenPreparingForMigrationsThenATableMustBeCreated()
        {
            var ct = CancellationToken.None;

            var manager = new SqlServerMigrationManager<MigratorTestContext>(
                _fixture.Context, new DefaultNamingNormalizer(), 
                LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());

            await manager.PrepareDatabaseAsync(ct);

            var tableId = await manager.Context.RunAsync(async () =>
                await manager.Context.QuerySingleAsync<long>(@"
select count(*) as Total
from (
	select 
        --  making sure all columns exist
        ContextName, MigrationId, ClassName, AppliedOn
	from __DbMigratorHistory
) V"), true, ct);
            Assert.NotNull(tableId);
            Assert.Equal(0L, tableId);
        }
    }
}