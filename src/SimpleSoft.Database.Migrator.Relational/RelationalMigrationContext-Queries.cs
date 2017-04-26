using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace SimpleSoft.Database.Migrator
{
    public partial class RelationalMigrationContext
    {
        /// <inheritdoc />
        public async Task<T> QuerySingleAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.QuerySingleAsync<T>(sql, param, Transaction, timeout, commandType);
        }

        /// <inheritdoc />
        public async Task<T> QueryFirstOrDefaultAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, Transaction, timeout, commandType);
        }

        /// <inheritdoc />
        public async Task<T> QuerySingleOrDefaultAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.QuerySingleOrDefaultAsync<T>(sql, param, Transaction, timeout, commandType);
        }
    }
}