using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Handlers
{
    /// <summary>
    /// The parameter for handlers that configure the <see cref="ILoggerFactory"/>
    /// for the host builder
    /// </summary>
    public sealed class LoggingConfiguratorParam
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="factory">The logger factory</param>
        /// <param name="configuration">The configuration</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoggingConfiguratorParam(ILoggerFactory factory, IConfiguration configuration)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            Factory = factory;
            Configuration = configuration;
        }

        /// <summary>
        /// The logger factory
        /// </summary>
        public ILoggerFactory Factory { get; }

        /// <summary>
        /// The host builder configuration
        /// </summary>
        public IConfiguration Configuration { get; }
    }
}