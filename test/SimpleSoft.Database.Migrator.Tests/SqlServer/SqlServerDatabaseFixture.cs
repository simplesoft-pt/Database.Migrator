using System;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public abstract class SqlServerDatabaseFixture : IDisposable
    {
        private readonly string _databaseName;
        private readonly bool _dropDatabase;

        protected SqlServerDatabaseFixture(
            string masterConnectionString, string connectionString, string databaseName, bool dropDatabase = false)
        {
            _databaseName = databaseName;
            _dropDatabase = dropDatabase;

            MasterConnectionString = masterConnectionString;
            MasterConnection = new SqlConnection(masterConnectionString);

            PrepareDatabase();

            ConnectionString = connectionString;
            Connection = new SqlConnection(connectionString);
        }

        ~SqlServerDatabaseFixture()
        {
            Dispose(false);
        }

        public string MasterConnectionString { get; }

        public DbConnection MasterConnection { get; private set; }

        public string ConnectionString { get; }

        public DbConnection Connection { get; private set; }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MasterConnection?.Dispose();
                Connection?.Dispose();
            }

            MasterConnection = null;
            Connection = null;
        }

        #endregion

        private void PrepareDatabase()
        {
            MasterConnection.Open();
            try
            {
                var timeout = MasterConnection.ConnectionTimeout;

                var dbId = MasterConnection.QuerySingleOrDefault<long?>(
                    "SELECT DB_ID(@DatabaseName) as DatabaseId", new
                    {
                        DatabaseName = _databaseName
                    }, null, timeout);
                if (dbId.HasValue)
                {
                    if (_dropDatabase)
                        MasterConnection.Execute("DROP DATABASE " + _databaseName, null, null, timeout);
                    else
                        return;
                }
                MasterConnection.Execute("CREATE DATABASE " + _databaseName, null, null, timeout);
            }
            finally
            {
                MasterConnection.Close();
            }
        }
    }
}