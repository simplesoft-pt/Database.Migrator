using System.Data;

namespace SimpleSoft.Database.Migrator.Tests.Relational
{
    public class MigratorTestContext : RelationalMigrationContext
    {
        /// <inheritdoc />
        public MigratorTestContext(IDbConnection connection) 
            : base(connection, LoggingManager.LoggerFactory)
        {

        }
    }
}