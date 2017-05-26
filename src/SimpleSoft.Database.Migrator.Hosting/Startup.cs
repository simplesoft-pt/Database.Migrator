using System;
using Microsoft.Extensions.DependencyInjection;
using SimpleSoft.Database.Migrator.Handlers;

namespace SimpleSoft.Database.Migrator.Hosting
{
    /// <summary>
    /// The hosting startup configuration
    /// </summary>
    public class Startup : IStartup
    {
        #region Implementation of IStartup

        /// <summary>
        /// Used to configure the <see cref="IHostingEnvironment"/> properties.
        /// </summary>
        /// <param name="environment">The hosting environment</param>
        public virtual void ConfigureHostingEnvironment(IHostingEnvironment environment)
        {

        }

        /// <summary>
        /// Used to append configuration providers to the configuration builder,
        /// like adding application settings.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        public virtual void ConfigureConfigurationBuilder(ConfigurationBuilderConfiguratorParam param)
        {

        }

        /// <summary>
        /// Used to append values to an already built root configurations.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        public virtual void ConfigureConfigurations(ConfigurationConfiguratorParam param)
        {

        }

        /// <summary>
        /// Used to configure the logging factory.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        public virtual void ConfigureLogging(LoggingConfiguratorParam param)
        {

        }

        /// <summary>
        /// Used to add services to the dependency injection container.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        public virtual void ConfigureServices(ServiceConfiguratorParam param)
        {

        }

        /// <summary>
        /// Used to build or replace the default service provider.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        /// <returns></returns>
        public virtual IServiceProvider BuildServiceProvider(ServiceProviderBuilderParam param)
        {
            return param.ServiceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Configure registered services.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        public virtual void Configure(ConfigureParam param)
        {

        }

        #endregion
    }
}