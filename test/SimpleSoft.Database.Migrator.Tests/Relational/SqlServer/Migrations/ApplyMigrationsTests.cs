using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer.Migrations
{
    /*
    public class ApplyMigrationsTests : IClassFixture<SqlMigrationDatabaseFixture>
    {
        private readonly SqlMigrationDatabaseFixture _fixture;

        public ApplyMigrationsTests(SqlMigrationDatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task GivenADatabaseWithoutMigrationsWhenApplyingAllThenEntriesMustExist()
        {
            var host = _fixture.HostBuilder.Build<ApplyMigrationsContext>();

            var ct = CancellationToken.None;

            await host.ApplyMigrationsAsync(ct);
            
            await _fixture.Connection.OpenAsync(ct);
            try
            {
                var totalMigrations = await _fixture.Connection.QuerySingleAsync<int>(@"
select 
    count(*) as TotalMigrations
from __DbMigratorHistory
where
    ContextName = @contextName and MigrationId in @migrationIds", new
                {
                    contextName = nameof(ApplyMigrationsContext),
                    migrationIds = host.MigrationMetadatas.Select(e => e.Id)
                });

                Assert.Equal(3, totalMigrations);

                var entries = (await _fixture.Connection.QueryAsync<dynamic>(@"
select 
    Id, Value, Description
from Version001Table")).ToList();

                Assert.NotEmpty(entries);
                Assert.Equal(15, entries.Count);
            }
            finally
            {
                _fixture.Connection.Close();
            }
        }
    }
    //*/
}
