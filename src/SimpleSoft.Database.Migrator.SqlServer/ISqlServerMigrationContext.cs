namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The SQL Server migration context
    /// </summary>
    public interface ISqlServerMigrationContext<out TOptions> : IRelationalMigrationContext
        where TOptions : SqlServerContextOptions
    {
        /// <summary>
        /// The database options
        /// </summary>
        TOptions Options { get; }
    }
}