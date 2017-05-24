using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator.Oracle
{
    /// <summary>
    /// Extensions for <see cref="MigrationsBuilder{TContext}"/> instances.
    /// </summary>
    public static class OracleMigrationsBuilderExtensions
    {
        /// <summary>
        /// Adds SQL Server support for the given <see cref="IRelationalMigrationContext"/>.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migration builder</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="config">An optional configuration handler</param>
        /// <returns>The builder after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static MigrationsBuilder<TContext> AddOracle<TContext>(
            this MigrationsBuilder<TContext> builder, string connectionString, Action<OracleContextOptions<TContext>> config = null)
            where TContext : class, IRelationalMigrationContext
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var options = new OracleContextOptions<TContext>(connectionString);
            config?.Invoke(options);
            builder.ServiceCollection.AddSingleton(k => options);
            builder.ServiceCollection.AddSingleton<OracleContextOptions>(
                k => k.GetRequiredService<OracleContextOptions<TContext>>());

            builder.ServiceCollection.AddScoped<OracleMigrationManager<TContext>>();
            builder.ServiceCollection.AddScoped<IOracleMigrationManager<TContext>>(
                k => k.GetRequiredService<OracleMigrationManager<TContext>>());
            builder.ServiceCollection.AddScoped<IMigrationManager<TContext>>(
                k => k.GetRequiredService<OracleMigrationManager<TContext>>());

            return builder;
        }
    }
}
