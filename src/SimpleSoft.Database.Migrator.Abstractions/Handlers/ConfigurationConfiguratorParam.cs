using System;
using Microsoft.Extensions.Configuration;

namespace SimpleSoft.Database.Migrator.Handlers
{
    /// <summary>
    /// The parameter for handlers that configure the <see cref="IConfigurationRoot"/>
    /// for the host builder
    /// </summary>
    public sealed class ConfigurationConfiguratorParam
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfigurationConfiguratorParam(IConfigurationRoot configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }

        /// <summary>
        /// The host builder configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; }
    }
}
