#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 João Simões
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Reflection;
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
        /// Registers an <see cref="IMigration{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="builder">The migration builder</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigration<TMigration, TContext>(
            this IServiceCollection services, Func<IServiceProvider, TMigration> builder)
            where TMigration : class, IMigration<TContext>
            where TContext : IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            services.AddScoped(builder);
            services.AddScoped<IMigration<TContext>>(builder);
            return services;
        }

        #endregion

        #region ScanMigrations

        /// <summary>
        /// Registers all <see cref="IMigration{TContext}"/> found in the given assembly.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection ScanMigrations(this IServiceCollection services, Assembly assembly)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers all <see cref="IMigration{TContext}"/> found in the given assemblies.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assemblies">The assemblies to scan</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection ScanMigrations(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            foreach (var assembly in assemblies)
                services.ScanMigrations(assembly);

            return services;
        }

        /// <summary>
        /// Registers all <see cref="IMigration{TContext}"/> found in the assembly of the given type.
        /// </summary>
        /// <typeparam name="T">The type to use</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection ScanMigrations<T>(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.ScanMigrations(typeof(T).GetTypeInfo().Assembly);
        }

        #endregion

        #region AddMigrationManager

        /// <summary>
        /// Registers a <see cref="IMigrationManager{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TManager">The manager type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrationManager<TManager, TContext>(this IServiceCollection services)
            where TManager : class, IMigrationManager<TContext>
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<TManager>();
            services.AddScoped<IMigrationManager<TContext>, TManager>();
            return services;
        }

        /// <summary>
        /// Registers a <see cref="IMigrationManager{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TManager">The manager type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="manager">The manager instance</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrationManager<TManager, TContext>(
            this IServiceCollection services, TManager manager)
            where TManager : class, IMigrationManager<TContext>
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            services.AddScoped(k => manager);
            services.AddScoped<IMigrationManager<TContext>>(k => manager);
            return services;
        }

        /// <summary>
        /// Registers a <see cref="IMigrationManager{TContext}"/> type to the services collection. 
        /// </summary>
        /// <typeparam name="TManager">The manager type</typeparam>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="builder">The manager builder</param>
        /// <returns>The service collection after registration</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrationManager<TManager, TContext>(
            this IServiceCollection services, Func<IServiceProvider, TManager> builder)
            where TManager : class, IMigrationManager<TContext>
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            services.AddScoped(builder);
            services.AddScoped<IMigrationManager<TContext>>(builder);
            return services;
        }

        #endregion

        /// <summary>
        /// Adds support for migrations for the given <see cref="IMigrationContext"/>.
        /// </summary>
        /// <typeparam name="TContext">The migration context</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="config">Configuration handler</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrations<TContext>(
            this IServiceCollection services, Action<MigrationsBuilder<TContext>> config)
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (config == null) throw new ArgumentNullException(nameof(config));

            var builder = new MigrationsBuilder<TContext>(services);
            config(builder);

            services.AddScoped<TContext>();
            foreach (var migrationType in builder.Migrations)
            {
                services.AddScoped(migrationType);
                services.AddScoped(typeof(IMigration<TContext>), migrationType);
            }

            //  TODO    this should be a configuration
            services.AddSingleton<INamingNormalizer>(new DefaultNamingNormalizer());

            return services;
        }
    }
}
