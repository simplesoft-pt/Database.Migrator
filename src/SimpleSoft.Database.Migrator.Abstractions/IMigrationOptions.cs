using System;
using System.Collections.Generic;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Options for a migration context.
    /// </summary>
    public interface IMigrationOptions
    {
        /// <summary>
        /// The context name.
        /// </summary>
        string ContextName { get; set; }

        /// <summary>
        /// The collection of known migrations.
        /// </summary>
        IReadOnlyCollection<Type> MigrationTypes { get; }

        /// <summary>
        /// Adds the migration to the collection.
        /// </summary>
        /// <param name="type">The migration type</param>
        void AddMigration(Type type);
    }

    /// <summary>
    /// Options for a migration context.
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public interface IMigrationOptions<in TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// Adds the migration to the collection.
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        void AddMigration<TMigration>() where TMigration : IMigration<TContext>;
    }
}