using System.Data;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class MigratorTestContext : RelationalMigrationContext
    {
        /// <inheritdoc />
        public MigratorTestContext(IDbConnection connection) 
            : base(connection, LoggingManager.CreateTestLogger<MigratorTestContext>())
        {

        }
    }
}