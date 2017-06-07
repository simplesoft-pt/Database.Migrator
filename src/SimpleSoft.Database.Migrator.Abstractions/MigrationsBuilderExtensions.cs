using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extension methods for <see cref="MigrationsBuilder{TContext}"/> instances
    /// </summary>
    public static class MigrationsBuilderExtensions
    {
        #region NamingNormalizer

        /// <summary>
        /// Uses the given naming normalizer for this context.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migrations builder</param>
        /// <param name="namingNormalizer">The normalizer to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MigrationsBuilder<TContext> UseNamingNormalizer<TContext>(
            this MigrationsBuilder<TContext> builder, INamingNormalizer<TContext> namingNormalizer)
            where TContext : class, IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (namingNormalizer == null) throw new ArgumentNullException(nameof(namingNormalizer));

            builder.ServiceCollection.AddSingleton(namingNormalizer);
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="DefaultNamingNormalizer{TContext}"/> as the normalizer for 
        /// this context.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migrations builder</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MigrationsBuilder<TContext> UseDefaultNamingNormalizer<TContext>(
            this MigrationsBuilder<TContext> builder)
            where TContext : class, IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddSingleton<INamingNormalizer<TContext>, DefaultNamingNormalizer<TContext>>();
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="DefaultNamingNormalizer{TContext}"/> as the normalizer for 
        /// this context.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migrations builder</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MigrationsBuilder<TContext> UseTrimNamingNormalizer<TContext>(
            this MigrationsBuilder<TContext> builder)
            where TContext : class, IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddSingleton<INamingNormalizer<TContext>, TrimNamingNormalizer<TContext>>();
            return builder;
        }

        #endregion

        #region AddMigrationsFrom

        /// <summary>
        /// Scans the assembly for all migrations that can be applied to the 
        /// given context, and adds them to the builder.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MigrationsBuilder<TContext> AddMigrationsFrom<TContext>(
            this MigrationsBuilder<TContext> builder, Assembly assembly)
            where TContext : IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var migrationBaseType = typeof(IMigration<TContext>).GetTypeInfo();

            var migrations = assembly.GetExportedTypes()
                .Where(e =>
                {
                    var t = e.GetTypeInfo();
                    return t.IsClass && !t.IsAbstract && migrationBaseType.IsAssignableFrom(t);
                });

            foreach (var migration in migrations)
            {
                builder.AddMigration(migration);
            }

            return builder;
        }

        /// <summary>
        /// Scans the assemblies for all migrations that can be applied to the 
        /// given context, and adds them to the builder.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="assemblies">The assemblies to scan</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MigrationsBuilder<TContext> AddMigrationsFrom<TContext>(
            this MigrationsBuilder<TContext> builder, params Assembly[] assemblies)
            where TContext : IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            foreach (var assembly in assemblies)
            {
                builder.AddMigrationsFrom(assembly);
            }

            return builder;
        }

        #endregion
    }
}
