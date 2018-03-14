namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Options for a SQL Server migration context.
    /// </summary>
    public interface ISqlServerMigrationOptions : IRelationalMigrationOptions
    {

    }

    /// <summary>
    /// Options for a SQL Server migration context.
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface ISqlServerMigrationOptions<in TContext> : ISqlServerMigrationOptions, IRelationalMigrationOptions<TContext>
        where TContext : ISqlServerMigrationContext
    {

    }
}