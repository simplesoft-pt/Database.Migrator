using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Relational.Internal;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public abstract class RelationalMigrationContext : MigrationContext, IRelationalMigrationContext, IDisposable
    {
        private readonly InternalRelationalMigrationContext _internalContext;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RelationalMigrationContext(IDbConnection connection, ILogger<RelationalMigrationContext> logger)
        {
            _internalContext = new InternalRelationalMigrationContext(connection, logger);
        }

        /// <inheritdoc />
        public IDbConnection Connection => _internalContext.Connection;

        /// <inheritdoc />
        public IDbTransaction Transaction => _internalContext.Transaction;

        /// <inheritdoc />
        public IsolationLevel DefaultIsolationLevel
        {
            get { return _internalContext.DefaultIsolationLevel; }
            set { _internalContext.DefaultIsolationLevel = value; }
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            _internalContext.Dispose();
        }

        #endregion

        #region Overrides of MigrationContext

        /// <inheritdoc />
        public override async Task PrepareAsync(CancellationToken ct)
        {
            await _internalContext.PrepareAsync(ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task PersistAsync(CancellationToken ct)
        {
            await _internalContext.PersistAsync(ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task RollbackAsync(CancellationToken ct)
        {
            await _internalContext.RollbackAsync(ct).ConfigureAwait(false);
        }

        #endregion
    }

    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public class RelationalMigrationContext<TOptions> : MigrationContext<TOptions>, IRelationalMigrationContext<TOptions>
        where TOptions : MigrationOptions
    {
        private readonly InternalRelationalMigrationContext _internalContext;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="options">The context options</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelationalMigrationContext(IDbConnection connection, TOptions options, ILogger<RelationalMigrationContext<TOptions>> logger) 
            : base(options)
        {
            _internalContext = new InternalRelationalMigrationContext(connection, logger);
        }

        /// <inheritdoc />
        public IDbConnection Connection => _internalContext.Connection;

        /// <inheritdoc />
        public IDbTransaction Transaction => _internalContext.Transaction;

        /// <summary>
        /// Isolation level
        /// </summary>
        public IsolationLevel DefaultIsolationLevel
        {
            get { return _internalContext.DefaultIsolationLevel; }
            set { _internalContext.DefaultIsolationLevel = value; }
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            _internalContext.Dispose();
        }

        #endregion

        #region Overrides of MigrationContext

        /// <inheritdoc />
        public override async Task PrepareAsync(CancellationToken ct)
        {
            await _internalContext.PrepareAsync(ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task PersistAsync(CancellationToken ct)
        {
            await _internalContext.PersistAsync(ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task RollbackAsync(CancellationToken ct)
        {
            await _internalContext.RollbackAsync(ct).ConfigureAwait(false);
        }

        #endregion
    }
}