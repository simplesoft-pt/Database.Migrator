using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer.ApplyMigrations
{
    public class ApplyMigrationsTests : IClassFixture<EmptySqlDatabaseFixture>
    {
        private readonly EmptySqlDatabaseFixture _fixture;

        public ApplyMigrationsTests(EmptySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        /*/
        [Fact]
        public async Task Tmp()
        {
            using (var builder = new MigratorHostBuilder().ConfigureLogging(p =>
            {
                p.Factory
                    .AddConsole(LogLevel.Trace, true)
                    .AddDebug(LogLevel.Trace);
            }).ConfigureServices(p =>
            {
                p.ServiceCollection
                    .AddMigrations<ApplyMigrationsContext>(options =>
                    {
                        options.AddSqlServer(EmptySqlDatabaseFixture.ConnectionString);

                        options.AddMigration<Version001>();
                        options.AddMigration<Version002>();
                        options.AddMigration<Version003>();
                    });
            }))
            {
                var host = builder.Build<ApplyMigrationsContext>();

                await host.ApplyMigrationsAsync(CancellationToken.None);
            }
        }
        //*/
    }

    public class ApplyMigrationsContext : RelationalMigrationContext
    {
        public ApplyMigrationsContext(ILogger<RelationalMigrationContext> logger) 
            : base(new SqlConnection(EmptySqlDatabaseFixture.ConnectionString), logger)
        {

        }
    }

    public class Version001 : Migration<ApplyMigrationsContext>
    {
        public Version001(ApplyMigrationsContext context) : base(context)
        {
            RunInTransaction = false;
        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
create table Version001Table(
	Id bigint primary key,
	Value nvarchar(256) not null
)");
        }

        #endregion
    }

    public class Version002 : Migration<ApplyMigrationsContext>
    {
        public Version002(ApplyMigrationsContext context) : base(context)
        {

        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            for (var i = 0; i < 15; i++)
            {
                await Context.ExecuteAsync(@"
insert into Version001Table(Id, Value) 
values (@id, @value)", new
                {
                    id = i,
                    value = "Some value " + i.ToString("D3")
                });
            }
        }

        #endregion
    }

    public class Version003 : Migration<ApplyMigrationsContext>
    {
        public Version003(ApplyMigrationsContext context) : base(context)
        {
            RunInTransaction = false;
        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
alter table Version001Table 
add Description nvarchar(1024) not null default 'hello world'");
        }

        #endregion
    }
}
