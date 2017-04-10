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

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The <see cref="IMigratorHost{TContext}"/> builder
    /// </summary>
    public interface IMigratorHostBuilder
    {
        /// <summary>
        /// Collection of handlers used to configure the <see cref="IConfiguration"/>.
        /// </summary>
        IReadOnlyCollection<Action<IConfiguration>> ConfigurationHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="ILoggerFactory"/>.
        /// </summary>
        IReadOnlyCollection<Action<ILoggerFactory, IConfiguration>> LoggingConfigurationHandlers { get; }

        /// <summary>
        /// Collection of handlers used to configure the <see cref="IServiceCollection"/>.
        /// </summary>
        IReadOnlyCollection<Action<IServiceCollection, ILoggerFactory, IConfiguration>> ServiceConfigurationHandlers { get; }

        /// <summary>
        /// Builder function for the <see cref="IServiceProvider"/>.
        /// </summary>
        Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> ServiceProviderBuilder { get; }

        /// <summary>
        /// Collection of handlers used to configure the services registered 
        /// into the <see cref="IServiceProvider"/>.
        /// </summary>
        IReadOnlyCollection<Action<IServiceProvider, ILoggerFactory, IConfiguration>> ConfigureHandlers { get; }

        /// <summary>
        /// Adds the handler to the <see cref="ConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurationConfigurator(Action<IConfiguration> handler);

        /// <summary>
        /// Adds the handler to the <see cref="LoggingConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddLoggingConfigurator(Action<ILoggerFactory, IConfiguration> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ServiceConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddServiceConfigurator(Action<IServiceCollection, ILoggerFactory, IConfiguration> handler);

        /// <summary>
        /// Adds the handler to the <see cref="ConfigureHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurator(Action<IServiceProvider, ILoggerFactory, IConfiguration> handler);

        /// <summary>
        /// Uses the given handler to build the <see cref="IServiceProvider"/> that
        /// will be used by the <see cref="IMigratorHost{TContext}"/> to build.
        /// </summary>
        /// <param name="buildServiceProvider">The builder function</param>
        void SetServiceProviderBuilder(Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> buildServiceProvider);

        /// <summary>
        /// Assigns the given <see cref="ILoggerFactory"/> to be used
        /// by the <see cref="IMigratorHost{TContext}"/>.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use</param>
        void SetLoggerFactory(ILoggerFactory loggerFactory);

        /// <summary>
        /// Gets a value from the configurations for the given key.
        /// </summary>
        /// <param name="key">The settings key</param>
        /// <returns>The key value or null if not found</returns>
        string GetSetting(string key);

        /// <summary>
        /// Sets or adds the value to the configurations by the given key
        /// </summary>
        /// <param name="key">The settings key</param>
        /// <param name="value">The value to assign</param>
        void SetSetting(string key, string value);

        /// <summary>
        /// Builds an instance of <see cref="IMigratorHost{TContext}"/> to run migrations
        /// for a given <see cref="IMigrationContext"/>.
        /// </summary>
        /// <returns>The <see cref="IMigratorHost{TContext}"/> instance</returns>
        IMigratorHost<TContext> Build<TContext>() where TContext : IMigrationContext;
    }
}
