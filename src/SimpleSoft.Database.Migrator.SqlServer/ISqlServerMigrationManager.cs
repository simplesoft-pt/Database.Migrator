namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states for SQL Server
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface ISqlServerMigrationManager<out TContext> : IRelationalMigrationManager<TContext>
        where TContext : IRelationalMigrationContext
    {
        
    }
}