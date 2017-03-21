using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public interface IMigrationContext<out TOptions> : IMigrationContext 
        where TOptions : MigrationOptions
    {
        /// <summary>
        /// The migration options
        /// </summary>
        TOptions Options { get; }
    }

    /// <summary>
    /// The migration context
    /// </summary>
    public interface IMigrationContext
    {
        /// <summary>
        /// Prepares the context to perform database work
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task PrepareAsync(CancellationToken ct);

        /// <summary>
        /// Persist all changes into the database
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task PersistAsync(CancellationToken ct);
    }
}