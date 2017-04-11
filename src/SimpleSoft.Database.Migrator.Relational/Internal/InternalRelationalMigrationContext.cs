using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational.Internal
{
    internal sealed class InternalRelationalMigrationContext : IDisposable
    {
        private static readonly Task CompletedTask = Task.FromResult(true);

        private readonly ILogger _logger;
        private bool _disposed;

        public InternalRelationalMigrationContext(DbConnection connection, ILogger logger)
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
        
        public DbConnection Connection { get; private set; }
        
        public IDbTransaction Transaction { get; private set; }

        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

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

            await Connection.OpenAsync(ct);

            Transaction = Connection.BeginTransaction(IsolationLevel);
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

        private void FailIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RelationalMigrationContext));
        }
    }
}
