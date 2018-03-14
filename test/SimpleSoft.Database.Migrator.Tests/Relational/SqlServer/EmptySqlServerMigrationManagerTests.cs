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

            var manager = new SqlServerMigrationManager<SqlServerMigratorTestContext>(
                _fixture.Context, LoggingManager.LoggerFactory);

            await manager.PrepareDatabaseAsync(ct);

            var tableId = await _fixture.Context.RunAsync(async () =>
                await _fixture.Context.QuerySingleAsync<long>(@"
select count(*) as Total
from (
	select 
        --  making sure all columns exist
        ContextName, MigrationId, ClassName, Description, AppliedOn
	from __DbMigratorHistory
) V", ct: ct), true, ct);
            Assert.Equal(0L, tableId);
        }
    }
}