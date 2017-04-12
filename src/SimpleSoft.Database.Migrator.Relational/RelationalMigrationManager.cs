using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// Manages migration states
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public abstract class RelationalMigrationManager<TContext> : MigrationManager<TContext> 
        where TContext : IRelationalMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RelationalMigrationManager(TContext context, 
            ILogger<RelationalMigrationManager<TContext>> logger) : base(context, logger)
        {

        }

        /// <summary>
        /// The migrations history table name
        /// </summary>
        public string MigrationsHistoryTableName { get; set; } = "__DbMigratorHistory";
    }
}
