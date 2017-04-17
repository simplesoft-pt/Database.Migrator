using System;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class EmptySqlDatabaseFixture : IDisposable
    {
        public const string DatabaseName = "MigratorTestEmpty";

        public const string ConnectionString =
                "Data Source=.; Initial Catalog=MigratorTestEmpty; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        private const string MasterConnectionString =
                "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        public EmptySqlDatabaseFixture()
        {
            DropRecreateDatabase(DatabaseName);

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