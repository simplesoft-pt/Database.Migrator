#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 João Simões
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Hosting.Handlers;

namespace SimpleSoft.Database.Migrator.Hosting
{
    /// <summary>
    /// The <see cref="IMigrationRunner{TContext}"/> builder
    /// </summary>
    public interface IMigratorHostBuilder
    {
        /// <summary>
        /// Collection of handlers used to configure the <see cref="IHostingEnvironment"/>.
        /// </summary>
        IReadOnlyCollection<Action<IHostingEnvironment>> HostingEnvironmentHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="IConfigurationBuilder"/>.
        /// </summary>
        IReadOnlyCollection<Action<ConfigurationBuilderConfiguratorParam>> ConfigurationBuilderHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="IConfiguration"/>.
        /// </summary>
        IReadOnlyCollection<Action<ConfigurationConfiguratorParam>> ConfigurationHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="ILoggerFactory"/>.
        /// </summary>
        IReadOnlyCollection<Action<LoggingConfiguratorParam>> LoggingConfigurationHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="IServiceCollection"/>.
        /// </summary>
        IReadOnlyCollection<Action<ServiceConfiguratorParam>> ServiceConfigurationHandlers { get; }

        /// <summary>
        /// Builder function for the <see cref="IServiceProvider"/>.
        /// </summary>
        Func<ServiceProviderBuilderParam, IServiceProvider> ServiceProviderBuilder { get; }

        /// <summary>
        /// Collection of handlers used to configure the services registered 
        /// into the <see cref="IServiceProvider"/>.
        /// </summary>
        IReadOnlyCollection<Action<ConfigureParam>> ConfigureHandlers { get; }

        /// <summary>
        /// Adds the handler to the <see cref="HostingEnvironmentHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddHostingEnvironmentConfigurator(Action<IHostingEnvironment> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ConfigurationBuilderHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurationBuilderConfigurator(Action<ConfigurationBuilderConfiguratorParam> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurationConfigurator(Action<ConfigurationConfiguratorParam> handler);

        /// <summary>
        /// Adds the handler to the <see cref="LoggingConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddLoggingConfigurator(Action<LoggingConfiguratorParam> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ServiceConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddServiceConfigurator(Action<ServiceConfiguratorParam> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ConfigureHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurator(Action<ConfigureParam> handler);

        /// <summary>
        /// Uses the given handler to build the <see cref="IServiceProvider"/> that
        /// will be used by the <see cref="IMigrationRunner{TContext}"/> to build.
        /// </summary>
        /// <param name="buildServiceProvider">The builder function</param>
        void SetServiceProviderBuilder(Func<ServiceProviderBuilderParam, IServiceProvider> buildServiceProvider);

        /// <summary>
        /// Assigns the given <see cref="ILoggerFactory"/> to be used
        /// by the <see cref="IMigrationRunner{TContext}"/>.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use</param>
        void SetLoggerFactory(ILoggerFactory loggerFactory);

        /// <summary>
        /// Builds an instance of <see cref="IMigrationRunner{TContext}"/> to run migrations
        /// for a given <see cref="IMigrationContext"/>.
        /// </summary>
        /// <returns>The <see cref="IMigrationRunner{TContext}"/> instance</returns>
        IMigrationRunner<TContext> Build<TContext>() where TContext : IMigrationContext;
    }
}
