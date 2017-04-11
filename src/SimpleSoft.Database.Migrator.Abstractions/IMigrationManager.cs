using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    public interface IMigrationManager<out TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// The migration context
        /// </summary>
        TContext Context { get; }

        /// <summary>
        /// Prepares the database for migrations support
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task PrepareDatabaseAsync(CancellationToken ct);

            /// <summary>
        /// Adds the given migration information to the database.
        /// </summary>
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="className">The migration class name</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<bool> AddMigrationAsync(string migrationId, string className, CancellationToken ct);

        /// <summary>
        /// Gets the most recent migration identifier from the database 
        /// or <code>null</code> if none is found.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct);

        /// <summary>
        /// Removes a migration from the database indicating success if
        /// the migration was found.
        /// </summary>
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<bool> RemoveMigrationAsync(string migrationId, CancellationToken ct);
    }
}