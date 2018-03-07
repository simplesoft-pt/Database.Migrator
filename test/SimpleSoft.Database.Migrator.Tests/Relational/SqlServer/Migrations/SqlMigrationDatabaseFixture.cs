
//using SimpleSoft.Database.Migrator.Hosting;

namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer.Migrations
{
    /*
    public class SqlMigrationDatabaseFixture : SqlServerDatabaseFixture
    {
        public SqlMigrationDatabaseFixture() : base(
            "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "Data Source=.; Initial Catalog=MigratorTestMigration; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "MigratorTestMigration", true)
        {
            HostBuilder = new MigratorHostBuilder()
                .ConfigureLogging(p =>
                {
                    p.Factory
                        .AddConsole(LogLevel.Trace, true)
                        .AddDebug(LogLevel.Trace);
                })
                .ConfigureServices(p =>
                {
                    p.ServiceCollection.AddMigrations<ApplyMigrationsContext>(config =>
                    {
                        config.AddSqlServer(ConnectionString);

                        config.AddMigration<Version001>();
                        config.AddMigration<Version002>();
                        config.AddMigration<Version003>();
                    });
                });
        }

        public MigratorHostBuilder HostBuilder { get; private set; }

        #region Overrides of SqlServerDatabaseFixture

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                HostBuilder?.Dispose();
            }

            HostBuilder = null;
        }

        #endregion
    }
    //*/
}