using System;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Relational extensions for <see cref="MigrationsBuilder{TContext}"/> instances.
    /// </summary>
    public static class RelationalMigrationsBuilderExtensions
    {
        /// <summary>
        /// Configures this context to be used for relational databases
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migrations builder</param>
        /// <param name="connectionString">The connection string to be used</param>
        /// <param name="config">The configuration handler</param>
        /// <returns></returns>
        public static RelationalMigrationsBuilder<TContext> AddRelational<TContext>(
            this MigrationsBuilder<TContext> builder, string connectionString, Action<RelationalMigrationsBuilder<TContext>> config)
            where TContext : IRelationalMigrationContext
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be whitespace.", nameof(connectionString));

            var relationalBuilder = new RelationalMigrationsBuilder<TContext>(builder.ServiceCollection, connectionString);
            foreach (var migrationType in builder.Migrations)
                relationalBuilder.AddMigration(migrationType);

            config(relationalBuilder);

            return relationalBuilder;
        }
    }
}
