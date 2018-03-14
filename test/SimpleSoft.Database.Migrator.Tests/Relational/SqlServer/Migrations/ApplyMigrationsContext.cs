namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer.Migrations
{
    public class ApplyMigrationsContext : SqlServerMigrationContext
    {
        /// <inheritdoc />
        public ApplyMigrationsContext(
            SqlServerMigrationOptions options, INamingNormalizer normalizer, IMigrationLoggerFactory loggerFactory)
            : base(options, normalizer, loggerFactory)
        {

        }
    }
}