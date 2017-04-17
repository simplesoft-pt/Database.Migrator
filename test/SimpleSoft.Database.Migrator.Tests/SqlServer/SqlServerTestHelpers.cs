using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public static class SqlServerTestHelpers
    {
        public const string DatabaseName = "MigratorTest";

        public const string MasterDatabaseName = "master";

        public const string ConnectionString =
                "Data Source=.; Initial Catalog=" + DatabaseName +
                "; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        public const string MasterConnectionString =
                "Data Source=.; Initial Catalog=" + MasterDatabaseName +
                "; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        public static async Task UsingConnectionForEmptyDatabaseAsync(
            Func<IDbConnection, CancellationToken, Task> action, CancellationToken ct = default(CancellationToken))
        {
            await DropRecreateDatabaseAsync(ct);

            using (var connection = new SqlConnection(ConnectionString))
            {
                await action(connection, ct);
            }
        }

        public static async Task UsingMigratorTestContextForEmptyDatabaseAsync(
            Func<MigratorTestContext, CancellationToken, Task> action,
            CancellationToken ct = default(CancellationToken))
        {
            await DropRecreateDatabaseAsync(ct);

            using (var connection = new SqlConnection(ConnectionString))
            using (var context = new MigratorTestContext(connection))
            {
                await action(context, ct);
            }
        }

        public static async Task UsingMigratorManagerAsync(
            Func<SqlServerMigrationManager<MigratorTestContext>, CancellationToken, Task> action,
            CancellationToken ct = default(CancellationToken))
        {
            await UsingMigratorTestContextForEmptyDatabaseAsync(async (ctx, c) =>
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(
                    ctx, LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());

                await manager.PrepareDatabaseAsync(c);

                await action(manager, c);
            }, ct);
        }

        private static async Task DropRecreateDatabaseAsync(CancellationToken ct)
        {
            using (var connection = new SqlConnection(MasterConnectionString))
            {
                var timeout = connection.ConnectionTimeout;

                await connection.OpenAsync(ct);

                var dbId = await connection.QuerySingleOrDefaultAsync<long?>(
                    "SELECT DB_ID(@databaseName) as DatabaseId", new
                    {
                        databaseName = DatabaseName
                    }, null, timeout);
                if (dbId.HasValue)
                    await connection.ExecuteAsync(
                        "DROP DATABASE " + DatabaseName, null, null, timeout);

                await connection.ExecuteAsync(
                    "CREATE DATABASE " + DatabaseName, null, null, timeout);
            }
        }
    }
}