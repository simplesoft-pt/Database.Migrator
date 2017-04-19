using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace SimpleSoft.Database.Migrator
{
    public partial class RelationalMigrationContext
    {
        /// <inheritdoc />
        public async Task<int> ExecuteAsync(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.ExecuteAsync(sql, param, Transaction, timeout, commandType);
        }

        /// <inheritdoc />
        public async Task<IDataReader> ExecuteReaderAsync(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.ExecuteReaderAsync(sql, param, Transaction, timeout, commandType);
        }

        /// <inheritdoc />
        public async Task<T> ExecuteScalarAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.ExecuteScalarAsync<T>(sql, param, Transaction, timeout, commandType);
        }
    }
}