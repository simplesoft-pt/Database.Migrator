using System.Data;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public interface IRelationalMigrationContext<out TOptions> : IMigrationContext<TOptions>, IRelationalMigrationContext 
        where TOptions : MigrationOptions
    {

    }

    /// <summary>
    /// The relational migration context
    /// </summary>
    public interface IRelationalMigrationContext : IMigrationContext
    {

    }
}
