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
                    .AddMigrationContext<ApplyMigrationsContext>()
                    .AddMigrationManager<SqlServerMigrationManager<ApplyMigrationsContext>, ApplyMigrationsContext>()
                    .AddMigration<Version001, ApplyMigrationsContext>()
                    .AddMigration<Version002, ApplyMigrationsContext>()
                    .AddMigration<Version003, ApplyMigrationsContext>();
            }))
            {
                var host = builder.Build<ApplyMigrationsContext>();

                await host.ApplyMigrationsAsync(CancellationToken.None);
            }
        }
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
        private readonly ILogger<Version001> _logger;

        public Version001(ApplyMigrationsContext context, ILogger<Version001> logger) : base(context)
        {
            _logger = logger;
        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            _logger.LogDebug("Running migration Version001");

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

        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            //await Context.ExecuteAsync("drop table Version001Table");
        }

        #endregion
    }
}
