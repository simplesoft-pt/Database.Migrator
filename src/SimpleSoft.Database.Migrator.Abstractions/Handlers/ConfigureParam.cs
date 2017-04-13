using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Handlers
{
    /// <summary>
    /// The parameter for handlers that configure the <see cref="IServiceProvider"/>
    /// for the host builder
    /// </summary>
    public sealed class ConfigureParam
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="factory">The logger factory</param>
        /// <param name="configuration">The configuration</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfigureParam(IServiceProvider serviceProvider, ILoggerFactory factory, IConfiguration configuration)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            ServiceProvider = serviceProvider;
            Factory = factory;
            Configuration = configuration;
        }

        /// <summary>
        /// The service collection
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

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