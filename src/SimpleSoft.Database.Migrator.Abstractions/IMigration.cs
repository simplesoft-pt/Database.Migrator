using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Represents a migration
    /// </summary>
    /// <typeparam name="TContext">Tme migration context</typeparam>
    public interface IMigration<out TContext>
        where TContext : IMigrationContext
    {
        /// <summary>
        /// The migration options
        /// </summary>
        TContext Context { get; }

        /// <summary>
        /// Applies the given migration into the database
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task ApplyAsync(CancellationToken ct);

        /// <summary>
        /// Does a rollback of this migration
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task RollbackAsync(CancellationToken ct);
    }

    /// <summary>
    /// Represents a migration
    /// </summary>
    public interface IMigration : IMigration<IMigrationContext>
    {
        
    }
}
