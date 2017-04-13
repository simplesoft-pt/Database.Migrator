using System;
using Microsoft.Extensions.Configuration;

namespace SimpleSoft.Database.Migrator.Handlers
{
    /// <summary>
    /// The parameter for handlers that configure the <see cref="IConfigurationBuilder"/>
    /// for the host builder
    /// </summary>
    public sealed class ConfigurationBuilderConfiguratorParam
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="builder">The configuration builder</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfigurationBuilderConfiguratorParam(IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            Builder = builder;
        }

        /// <summary>
        /// the configuration builder
        /// </summary>
        public IConfigurationBuilder Builder { get; }
    }
}