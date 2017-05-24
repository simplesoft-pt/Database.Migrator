namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface IRelationalMigrationManager<out TContext> : IMigrationManager<TContext>
        where TContext : IRelationalMigrationContext
    {
        /// <summary>
        /// The migrations history table name
        /// </summary>
        string MigrationsHistoryTableName { get; set; }
    }
}