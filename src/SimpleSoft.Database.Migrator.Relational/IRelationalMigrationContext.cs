using System.Data;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public interface IRelationalMigrationContext : IMigrationContext
    {
        /// <summary>
        /// The database connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// The current transaction or null
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Isolation level
        /// </summary>
        IsolationLevel DefaultIsolationLevel { get; }

        #region QuerySingleAsync

        /// <summary>
        /// Queries a single result from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> QuerySingleAsync<T>(
            string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null);

        #endregion
    }

    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public interface IRelationalMigrationContext<out TOptions> : IMigrationContext<TOptions>, IRelationalMigrationContext 
        where TOptions : MigrationOptions
    {

    }
}
