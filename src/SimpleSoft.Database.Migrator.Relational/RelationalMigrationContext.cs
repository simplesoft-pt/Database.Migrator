using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public abstract class RelationalMigrationContext : MigrationContext, IRelationalMigrationContext, IDisposable
    {
        private static readonly Task CompletedTask = Task.FromResult(true);

        private readonly ILogger<RelationalMigrationContext> _logger;
        private bool _disposed;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RelationalMigrationContext(IDbConnection connection, ILogger<RelationalMigrationContext> logger)
        {
            _logger = logger;
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            Connection = connection;
        }

        /// <inheritdoc />
        ~RelationalMigrationContext()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public IDbConnection Connection { get; private set; }

        /// <inheritdoc />
        public IDbTransaction Transaction { get; private set; }

        /// <inheritdoc />
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
        public override async Task PrepareAsync(CancellationToken ct)
        {
            FailIfDisposed();

            var dbConnection = Connection as DbConnection;
            if (dbConnection == null)
                Connection.Open();
            else
                await dbConnection.OpenAsync(ct).ConfigureAwait(false);

            Transaction = Connection.BeginTransaction(DefaultIsolationLevel);
        }

        /// <inheritdoc />
        public override Task PersistAsync(CancellationToken ct)
        {
            FailIfDisposed();

            Transaction.Commit();

            return CompletedTask;
        }

        /// <inheritdoc />
        public override Task RollbackAsync(CancellationToken ct)
        {
            FailIfDisposed();

            Transaction.Rollback();

            return CompletedTask;
        }

        #endregion

        #region QuerySingleAsync

        /// <inheritdoc />
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

    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public class RelationalMigrationContext<TOptions> : RelationalMigrationContext, IRelationalMigrationContext<TOptions>
        where TOptions : MigrationOptions
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="options">The context options</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelationalMigrationContext(TOptions options, IDbConnection connection, ILogger<RelationalMigrationContext<TOptions>> logger) 
            : base(connection, logger)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options;
        }

        #region Implementation of IMigrationContext<out TOptions>

        /// <inheritdoc />
        public TOptions Options { get; }

        #endregion
    }
}