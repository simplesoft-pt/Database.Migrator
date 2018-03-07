namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer.Migrations
{
    public class ApplyMigrationsContext : SqlServerMigrationContext<SqlServerContextOptions<ApplyMigrationsContext>>
    {
        /// <inheritdoc />
        public ApplyMigrationsContext(
            SqlServerContextOptions<ApplyMigrationsContext> options, IMigrationLoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {

        }
    }
}