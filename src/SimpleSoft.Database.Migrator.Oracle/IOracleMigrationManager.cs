namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states for Oracle
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface IOracleMigrationManager<out TContext> : IRelationalMigrationManager<TContext>
        where TContext : IRelationalMigrationContext
    {

    }
}
