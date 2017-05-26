using System;
using SimpleSoft.Database.Migrator.Hosting.Handlers;

namespace SimpleSoft.Database.Migrator.Hosting
{
    /// <summary>
    /// The hosting startup configuration
    /// </summary>
    public interface IMigratorHostStartup
    {
        /// <summary>
        /// Used to configure the <see cref="IHostingEnvironment"/> properties.
        /// </summary>
        /// <param name="environment">The hosting environment</param>
        void ConfigureHostingEnvironment(IHostingEnvironment environment);

        /// <summary>
        /// Used to append configuration providers to the configuration builder,
        /// like adding application settings.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        void ConfigureConfigurationBuilder(ConfigurationBuilderConfiguratorParam param);

        /// <summary>
        /// Used to append values to an already built root configurations.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        void ConfigureConfigurations(ConfigurationConfiguratorParam param);

        /// <summary>
        /// Used to configure the logging factory.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        void ConfigureLogging(LoggingConfiguratorParam param);

        /// <summary>
        /// Used to add services to the dependency injection container.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        void ConfigureServices(ServiceConfiguratorParam param);

        /// <summary>
        /// Used to build or replace the default service provider.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        /// <returns></returns>
        IServiceProvider BuildServiceProvider(ServiceProviderBuilderParam param);

        /// <summary>
        /// Configure registered services.
        /// </summary>
        /// <param name="param">The handler parameter</param>
        void Configure(ConfigureParam param);
    }
}
