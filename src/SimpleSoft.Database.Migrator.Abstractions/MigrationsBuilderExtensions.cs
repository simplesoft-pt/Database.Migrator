using System;
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
        /// <returns></returns>
        public static MigrationsBuilder<TContext> UseNamingNormalizer<TContext>(
            this MigrationsBuilder<TContext> builder, INamingNormalizer<TContext> namingNormalizer)
            where TContext : class, IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddSingleton(namingNormalizer);
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="DefaultNamingNormalizer{TContext}"/> as the normalizer for 
        /// this context.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migrations builder</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static MigrationsBuilder<TContext> UseTrimNamingNormalizer<TContext>(
            this MigrationsBuilder<TContext> builder)
            where TContext : class, IMigrationContext
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddSingleton<INamingNormalizer<TContext>, TrimNamingNormalizer<TContext>>();
            return builder;
        }

        #endregion
    }
}
