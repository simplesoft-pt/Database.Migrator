using System;
using Microsoft.Extensions.Configuration;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extension methods for <see cref="IMigratorHostBuilder"/>
    /// </summary>
    public static class ConfigurationExtensions
    {
        #region Environment

        /// <summary>
        /// Gets the environment value from the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <returns>The environment value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Environment(this IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration[MigratorHostDefaults.EnvironmentKey];
        }

        /// <summary>
        /// Sets the environment value into the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <param name="environment">The environment value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void Environment(this IConfiguration configuration, string environment)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (environment == null) throw new ArgumentNullException(nameof(environment));
            if (string.IsNullOrWhiteSpace(environment))
                throw new ArgumentException("Value cannot be whitespace.", nameof(environment));

            configuration[MigratorHostDefaults.EnvironmentKey] = environment;
        }

        #endregion

        #region ContentRoot

        /// <summary>
        /// Gets the content root value from the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <returns>The environment value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ContentRoot(this IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration[MigratorHostDefaults.ContentRootKey];
        }

        /// <summary>
        /// Sets the content root value into the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <param name="contentRoot">The content root value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ContentRoot(this IConfiguration configuration, string contentRoot)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            configuration[MigratorHostDefaults.ContentRootKey] = contentRoot;
        }

        #endregion
    }
}
