namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Options for a relational migration context.
    /// </summary>
    public interface IRelationalMigrationOptions : IMigrationOptions
    {
        /// <summary>
        /// The database connection string.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// The table name.
        /// </summary>
        string TableName { get; }
    }

    /// <summary>
    /// Options for a relational migration context.
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface IRelationalMigrationOptions<in TContext> : IRelationalMigrationOptions, IMigrationOptions<TContext>
        where TContext : IRelationalMigrationContext
    {

    }
}