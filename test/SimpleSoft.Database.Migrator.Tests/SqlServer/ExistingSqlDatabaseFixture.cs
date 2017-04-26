using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using Dapper;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class ExistingSqlDatabaseFixture : IDisposable
    {
        public const string DatabaseName = "MigratorTest";

        public const string ConnectionString =
                "Data Source=.; Initial Catalog=MigratorTest; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        private const string MasterConnectionString =
                "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        public ExistingSqlDatabaseFixture()
        {
            CreateDatabaseIfNeeded(DatabaseName);

            Connection = new SqlConnection(ConnectionString);
            Context = new MigratorTestContext(Connection);
        }

        public DbConnection Connection { get; private set; }

        public MigratorTestContext Context { get; private set; }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Context?.Dispose();
            Connection?.Dispose();

            Context = null;
            Connection = null;
        }

        #endregion

        private static void CreateDatabaseIfNeeded(string databaseName)
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
                    return;

                connection.Execute("CREATE DATABASE " + databaseName, null, null, timeout);
            }

            var ct = CancellationToken.None;
            using (var connection = new SqlConnection(ConnectionString))
            using (var context = new MigratorTestContext(connection))
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(
                    context, LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());

                manager.PrepareDatabaseAsync(ct)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                //  Existing static migration
                manager.AddMigrationAsync("V00000", "SimpleSoft.Database.Migrator.Tests.SqlServer.V00000", ct)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}