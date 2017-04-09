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
            services.AddScoped<IMigrationContext, TContext>();
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
            services.AddScoped<IMigrationContext, TContext>(k => context);
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
            services.AddScoped<IMigrationContext, TContext>(builder);
            return services;
        }

        #endregion

        #region AddMigration

        /// <summary>
        /// Registers an <see cref="IMigration{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration, TContext>(this IServiceCollection services)
            where TMigration : class, IMigration<TContext> 
            where TContext : IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<TMigration>();
            services.AddScoped<IMigration<TContext>, TMigration>();
            return services;
        }

        /// <summary>
        /// Registers an <see cref="IMigration"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration>(this IServiceCollection services)
            where TMigration : class, IMigration
        {
            return services.AddMigration<TMigration, IMigrationContext>()
                .AddScoped<IMigration, TMigration>();
        }

        /// <summary>
        /// Registers an <see cref="IMigration{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="migration">The migration instance</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration, TContext>(
            this IServiceCollection services, TMigration migration)
            where TMigration : class, IMigration<TContext> 
            where TContext : IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (migration == null) throw new ArgumentNullException(nameof(migration));

            services.AddScoped(k => migration);
            services.AddScoped<IMigration<TContext>>(k => migration);
            return services;
        }

        /// <summary>
        /// Registers an <see cref="IMigration"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="migration">The migration instance</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration>(
            this IServiceCollection services, TMigration migration)
            where TMigration : class, IMigration
        {
            return services.AddMigration<TMigration, IMigrationContext>(migration)
                .AddScoped<IMigration>(k => migration);
        }

        /// <summary>
        /// Registers an <see cref="IMigration{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="builder">The migration builder</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration, TContext>(
            this IServiceCollection services, Func<IServiceProvider,TMigration> builder)
            where TMigration : class, IMigration<TContext> 
            where TContext : IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            services.AddScoped(builder);
            services.AddScoped<IMigration<TContext>>(builder);
            return services;
        }

        /// <summary>
        /// Registers an <see cref="IMigration{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="builder">The migration builder</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration>(
            this IServiceCollection services, Func<IServiceProvider, TMigration> builder)
            where TMigration : class, IMigration
        {
            return services.AddMigration<TMigration, IMigrationContext>(builder)
                .AddScoped<IMigration>(builder);
        }

        #endregion
    }
}
