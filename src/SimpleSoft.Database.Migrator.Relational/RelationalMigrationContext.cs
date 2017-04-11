using System;
using System.Data;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public abstract class RelationalMigrationContext : MigrationContext, IRelationalMigrationContext
    {
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

        #region Implementation of IRelationalMigrationContext

        /// <inheritdoc />
        public IDbConnection Connection { get; }

        #endregion
    }

    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public class RelationalMigrationContext<TOptions> : MigrationContext<TOptions>, IRelationalMigrationContext<TOptions>
        where TOptions : MigrationOptions
    {
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

        #region Implementation of IRelationalMigrationContext

        /// <inheritdoc />
        public IDbConnection Connection { get; }

        #endregion
    }
}