namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The Oracle migration context
    /// </summary>
    public interface IOracleMigrationContext<out TOptions> : IRelationalMigrationContext
        where TOptions : OracleContextOptions
    {
        /// <summary>
        /// The database options
        /// </summary>
        TOptions Options { get; }
    }
}