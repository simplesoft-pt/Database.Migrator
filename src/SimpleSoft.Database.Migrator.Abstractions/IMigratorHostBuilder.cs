using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The <see cref="IMigratorHost"/> builder
    /// </summary>
    public interface IMigratorHostBuilder
    {
        /// <summary>
        /// Collection of handlers used to configure the <see cref="ILoggerFactory"/>.
        /// </summary>
        IReadOnlyCollection<Action<ILoggerFactory>> LoggingConfigurationHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="IServiceCollection"/>.
        /// </summary>
        IReadOnlyCollection<Action<IServiceCollection, ILoggerFactory>> ServiceConfigurationHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the services registered 
        /// into the <see cref="IServiceProvider"/>.
        /// </summary>
        IReadOnlyCollection<Action<IServiceProvider, ILoggerFactory>> ConfigurationHandlers { get; }

        /// <summary>
        /// Builder function for the <see cref="IServiceProvider"/>.
        /// </summary>
        Func<IServiceCollection, ILoggerFactory, IServiceProvider> ServiceProviderBuilder { get; }

        /// <summary>
        /// Adds the handler to the <see cref="LoggingConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddLoggingConfigurator(Action<ILoggerFactory> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ServiceConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddServiceConfigurator(Action<IServiceCollection, ILoggerFactory> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurator(Action<IServiceProvider, ILoggerFactory> handler);

        /// <summary>
        /// Uses the given handler to build the <see cref="IServiceProvider"/> that
        /// will be used by the <see cref="IMigratorHost"/> to build.
        /// </summary>
        /// <param name="buildServiceProvider">The builder function</param>
        void SetServiceProviderBuilder(Func<IServiceCollection, ILoggerFactory, IServiceProvider> buildServiceProvider);

        /// <summary>
        /// Assigns the given <see cref="ILoggerFactory"/> to be used
        /// by the <see cref="IMigratorHost"/>.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use</param>
        void SetLoggerFactory(ILoggerFactory loggerFactory);

        /// <summary>
        /// Builds an instance of <see cref="IMigratorHost"/> to run migrations.
        /// </summary>
        /// <returns>The <see cref="IMigratorHost"/> instance</returns>
        IMigratorHost Build();
    }
}
