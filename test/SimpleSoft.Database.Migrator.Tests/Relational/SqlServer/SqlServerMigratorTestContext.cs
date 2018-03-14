namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer
{
    public class SqlServerMigratorTestContext : SqlServerMigrationContext
    {
        public SqlServerMigratorTestContext(ISqlServerMigrationOptions options) 
            : base(options, new DefaultNamingNormalizer(), LoggingManager.LoggerFactory)
        {

        }
    }
}