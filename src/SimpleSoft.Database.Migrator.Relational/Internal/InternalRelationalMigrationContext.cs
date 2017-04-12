using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational.Internal
{
    internal sealed class InternalRelationalMigrationContext : IDisposable
    {
        private static readonly Task CompletedTask = Task.FromResult(true);

        private readonly ILogger _logger;
        private bool _disposed;

        public InternalRelationalMigrationContext(IDbConnection connection, ILogger logger)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;

            Connection = connection;
        }

        /// <inheritdoc />
        ~InternalRelationalMigrationContext()
        {
            Dispose(false);
        }
        
        public IDbConnection Connection { get; private set; }
        
        public IDbTransaction Transaction { get; private set; }

        public IsolationLevel DefaultIsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Transaction.Dispose();
                Connection.Dispose();
            }

            Transaction = null;
            Connection = null;

            _disposed = true;
        }

        #endregion

        #region Overrides of MigrationContext

        /// <inheritdoc />
        public async Task PrepareAsync(CancellationToken ct)
        {
            FailIfDisposed();

            var dbConnection = Connection as DbConnection;
            if (dbConnection == null)
                Connection.Open();
            else
                await dbConnection.OpenAsync(ct);

            Transaction = Connection.BeginTransaction(DefaultIsolationLevel);
        }

        /// <inheritdoc />
        public Task PersistAsync(CancellationToken ct)
        {
            FailIfDisposed();

            Transaction.Commit();

            return CompletedTask;
        }

        /// <inheritdoc />
        public Task RollbackAsync(CancellationToken ct)
        {
            FailIfDisposed();

            Transaction.Rollback();

            return CompletedTask;
        }

        #endregion

        #region QuerySingleAsync

        public async Task<T> QuerySingleAsync<T>(
            string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            int timeout;
            AssertCommandParameters(
                transaction, commandTimeout, out transaction, out timeout);

            LogQuery(sql, transaction, timeout);

            return await Connection.QuerySingleAsync<T>(sql, param, transaction, timeout, commandType);
        }

        #endregion

        private void FailIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RelationalMigrationContext));
        }

        private void LogQuery(string query, IDbTransaction transaction, int commandTimeout)
        {
            _logger.LogDebug(@"
Executing sql statement in database.
    Is in transaction? {isInTransaction}
    Command Timeout: {commandTimeout}

SQL to execute:
{sqlStatement}",
                transaction != null, commandTimeout, query);
        }

        private void AssertCommandParameters(
            IDbTransaction transaction, int? commandTimeout, out IDbTransaction tx, out int timeout)
        {
            tx = transaction ?? Transaction;
            timeout = commandTimeout ?? Connection.ConnectionTimeout;
        }
    }
}
