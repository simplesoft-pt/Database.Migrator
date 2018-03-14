namespace SimpleSoft.Database.Migrator.Tests.Relational.Oracle
{
    public class OracleMigratorTestContext : OracleMigrationContext
    {
        public OracleMigratorTestContext(IOracleMigrationOptions options)
            : base(options, new DefaultNamingNormalizer(), LoggingManager.LoggerFactory)
        {

        }
    }
}