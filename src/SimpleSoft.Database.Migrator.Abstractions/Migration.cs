using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Represents a migration
    /// </summary>
    /// <typeparam name="TContext">The migration context</typeparam>
    public abstract class Migration<TContext> : IMigration<TContext> 
        where TContext : IMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Migration(TContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Context = context;
        }

        /// <inheritdoc />
        public TContext Context { get; }

        /// <inheritdoc />
        public virtual Task ApplyAsync(CancellationToken ct)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public virtual Task RollbackAsync(CancellationToken ct)
        {
            return Task.FromResult(true);
        }
    }

    /// <summary>
    /// Represents a migration
    /// </summary>
    public abstract class Migration: Migration<IMigrationContext>, IMigration
    {
        /// <inheritdoc />
        protected Migration(IMigrationContext context) : base(context)
        {

        }
    }
}