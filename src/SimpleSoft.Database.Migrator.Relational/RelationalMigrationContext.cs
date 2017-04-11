using System;
using System.Data;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public abstract class RelationalMigrationContext : MigrationContext, IRelationalMigrationContext, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RelationalMigrationContext(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            Connection = connection;
        }

        /// <inheritdoc />
        ~RelationalMigrationContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// The database connection
        /// </summary>
        public IDbConnection Connection { get; private set; }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Invoked when disposing the instance.
        /// </summary>
        /// <param name="disposing">True if disposing, otherwise false</param>
        protected virtual void Dispose(bool disposing)
        {
            if(_disposed) return;

            if (disposing)
                Connection?.Dispose();

            Connection = null;
            _disposed = true;
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
        private bool _disposed;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="options">The context options</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelationalMigrationContext(IDbConnection connection, TOptions options) : base(options)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /// <inheritdoc />
        ~RelationalMigrationContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// The database connection
        /// </summary>
        public IDbConnection Connection { get; private set; }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Invoked when disposing the instance.
        /// </summary>
        /// <param name="disposing">True if disposing, otherwise false</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                Connection?.Dispose();

            Connection = null;
            _disposed = true;
        }

        #endregion
    }
}