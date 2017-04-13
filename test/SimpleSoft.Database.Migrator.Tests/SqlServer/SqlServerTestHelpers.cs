using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public static class SqlServerTestHelpers
    {
        public const string ConnectionString =
                "Data Source=.; Initial Catalog=MigratorTest; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer"
            ;

        public static async Task UsingConnectionAsync(
            Func<IDbConnection, CancellationToken, Task> action, CancellationToken ct = default(CancellationToken))
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await action(connection, ct);
            }
        }
    }
}