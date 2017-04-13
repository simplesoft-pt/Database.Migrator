using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Handlers
{
    /// <summary>
    /// The parameter to the <see cref="IServiceProvider"/> builder
    /// for the host builder
    /// </summary>
    public class ServiceProviderBuilderParam
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceCollection">The service collection</param>
        /// <param name="factory">The logger factory</param>
        /// <param name="configuration">The configuration</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ServiceProviderBuilderParam(IServiceCollection serviceCollection, ILoggerFactory factory, IConfiguration configuration)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            ServiceCollection = serviceCollection;
            Factory = factory;
            Configuration = configuration;
        }

        /// <summary>
        /// The service collection
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

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