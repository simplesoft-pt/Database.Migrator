using System;

namespace SimpleSoft.Database.Migrator.Hosting
{
    /// <summary>
    /// Extensions for <see cref="IHostingEnvironment"/>.
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current hosting environment name is "Development".
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment</param>
        /// <returns>True if the environment name is "Development", otherwise false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsDevelopment(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));

            return "Development".Equals(hostingEnvironment.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current hosting environment name is "Staging".
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment</param>
        /// <returns>True if the environment name is "Staging", otherwise false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsStaging(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));

            return "Staging".Equals(hostingEnvironment.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current hosting environment name is "Production".
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment</param>
        /// <returns>True if the environment name is "Production", otherwise false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsProduction(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));

            return "Production".Equals(hostingEnvironment.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares the current hosting environment name against the specified value.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment</param>
        /// <param name="environmentName">The environment name</param>
        /// <returns>True if the specified name is the same as the current environment, otherwise false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsEnvironment(this IHostingEnvironment hostingEnvironment, string environmentName)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));
            if (environmentName == null)
                throw new ArgumentNullException(nameof(environmentName));
            if (string.IsNullOrWhiteSpace(environmentName))
                throw new ArgumentException("Value cannot be whitespace.", nameof(environmentName));

            return environmentName.Equals(hostingEnvironment.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
