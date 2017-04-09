using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extensions to help register services into the migrator
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region AddMigrationContext

        /// <summary>
        /// Registers a <see cref="IMigrationContext"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrationContext<TContext>(this IServiceCollection services)
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<TContext>();
            return services;
        }

        /// <summary>
        /// Registers a <see cref="IMigrationContext"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="context">The migration context instance</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrationContext<TContext>(
            this IServiceCollection services, TContext context)
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (context == null) throw new ArgumentNullException(nameof(context));

            services.AddScoped(k => context);
            return services;
        }

        /// <summary>
        /// Registers a <see cref="IMigrationContext"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="builder">The migration context builder</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrationContext<TContext>(
            this IServiceCollection services, Func<IServiceProvider, TContext> builder)
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            services.AddScoped(builder);
            return services;
        }

        #endregion
    }
}
