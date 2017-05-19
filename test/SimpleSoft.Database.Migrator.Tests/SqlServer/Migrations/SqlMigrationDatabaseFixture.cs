using System;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer.Migrations
{
    public class SqlMigrationDatabaseFixture : IDisposable
    {
        public const string DatabaseName = "MigratorTestMigration";

        public const string ConnectionString =
                "Data Source=.; Initial Catalog=MigratorTestMigration; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        private const string MasterConnectionString =
                "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        public SqlMigrationDatabaseFixture()
        {
            DropRecreateDatabase(DatabaseName);

            Connection = new SqlConnection(ConnectionString);
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

        public DbConnection Connection { get; private set; }

        public MigratorHostBuilder HostBuilder { get; private set; }


        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Connection?.Dispose();
            HostBuilder?.Dispose();

            Connection = null;
            HostBuilder = null;
        }

        #endregion

        private static void DropRecreateDatabase(string databaseName)
        {
            using (var connection = new SqlConnection(MasterConnectionString))
            {
                connection.Open();

                var timeout = connection.ConnectionTimeout;

                var dbId = connection.QuerySingleOrDefault<long?>(
                    "SELECT DB_ID(@databaseName) as DatabaseId", new
                    {
                        databaseName
                    }, null, timeout);
                if (dbId.HasValue)
                    connection.Execute("DROP DATABASE " + databaseName, null, null, timeout);

                connection.Execute("CREATE DATABASE " + databaseName, null, null, timeout);
            }
        }
    }
}