using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The migration context
    /// </summary>
    public abstract class MigrationContext : IMigrationContext
    {
        #region Implementation of IMigrationContext

        /// <inheritdoc />
        public virtual Task PrepareAsync(CancellationToken ct)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public virtual Task PersistAsync(CancellationToken ct)
        {
            return Task.FromResult(true);
        }

        #endregion
    }

    /// <summary>
    /// The migration context
    /// </summary>
    public abstract class MigrationContext<TOptions> : MigrationContext, IMigrationContext<TOptions> 
        where TOptions : MigrationOptions
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="options">The context options</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MigrationContext(TOptions options)
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
