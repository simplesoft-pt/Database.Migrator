namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Options for an Oracle migration context.
    /// </summary>
    public interface IOracleMigrationOptions : IRelationalMigrationOptions
    {

    }

    /// <summary>
    /// Options for an Oracle migration context.
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface IOracleMigrationOptions<in TContext> : IOracleMigrationOptions, IRelationalMigrationOptions<TContext>
        where TContext : IOracleMigrationContext
    {

    }
}