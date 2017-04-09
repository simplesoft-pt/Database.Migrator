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
    /// The <see cref="IMigratorHost"/> builder
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
        /// Collection of handlers used to configure the services registered 
        /// into the <see cref="IServiceProvider"/>.
        /// </summary>
        IReadOnlyCollection<Action<IServiceProvider, ILoggerFactory, IConfiguration>> ConfigureHandlers { get; }

        /// <summary>
        /// Builder function for the <see cref="IServiceProvider"/>.
        /// </summary>
        Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> ServiceProviderBuilder { get; }

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
        /// will be used by the <see cref="IMigratorHost"/> to build.
        /// </summary>
        /// <param name="buildServiceProvider">The builder function</param>
        void SetServiceProviderBuilder(Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> buildServiceProvider);

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
