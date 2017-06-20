using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace SimpleSoft.Database.Migrator
{
    public partial class RelationalMigrationContext
    {
        /// <inheritdoc />
        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null,
            CancellationToken ct = default(CancellationToken))
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            return await Connection.QueryAsync<T>(
                    new CommandDefinition(sql, param, Transaction, timeout, commandType, cancellationToken: ct))
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> QuerySingleAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null,
            CancellationToken ct = default(CancellationToken))
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            ct.ThrowIfCancellationRequested();
            return await Connection.QuerySingleAsync<T>(sql, param, Transaction, timeout, commandType)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> QuerySingleOrDefaultAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null,
            CancellationToken ct = default(CancellationToken))
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            ct.ThrowIfCancellationRequested();
            return await Connection.QuerySingleOrDefaultAsync<T>(sql, param, Transaction, timeout, commandType)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> QueryFirstAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null,
            CancellationToken ct = default(CancellationToken))
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            ct.ThrowIfCancellationRequested();
            return await Connection.QueryFirstAsync<T>(sql, param, Transaction, timeout, commandType)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> QueryFirstOrDefaultAsync<T>(
            string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null,
            CancellationToken ct = default(CancellationToken))
        {
            FailIfDisposed();

            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, timeout);

            ct.ThrowIfCancellationRequested();
            return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, Transaction, timeout, commandType)
                .ConfigureAwait(false);
        }
    }
}