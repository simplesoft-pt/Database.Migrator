using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer.Migrations
{
    public class ApplyMigrationsContext : SqlServerMigrationContext<SqlServerContextOptions<ApplyMigrationsContext>>
    {
        /// <inheritdoc />
        public ApplyMigrationsContext(
            SqlServerContextOptions<ApplyMigrationsContext> options, ILogger<ApplyMigrationsContext> logger)
            : base(options, logger)
        {

        }
    }
}