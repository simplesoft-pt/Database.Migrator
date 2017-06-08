using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The migration registration builder for relational databases
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class RelationalMigrationsBuilder<TContext> : MigrationsBuilder<TContext>
        where TContext : IRelationalMigrationContext
    {
        /// <inheritdoc />
        public RelationalMigrationsBuilder(IServiceCollection serviceCollection, string connectionString) 
            : base(serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be whitespace.", nameof(connectionString));

            ConnectionString = connectionString;
        }

        /// <summary>
        /// The database connection string
        /// </summary>
        public string ConnectionString { get; }
    }
}