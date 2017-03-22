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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The <see cref="IMigratorHost"/> builder
    /// </summary>
    public class MigratorHostBuilder : IMigratorHostBuilder, IDisposable
    {
        private ILoggerFactory _loggerFactory;
        private bool _disposed;
        private readonly List<Action<ILoggerFactory>> _loggingConfigurationHandlers;
        private readonly List<Action<IServiceCollection, ILoggerFactory>> _serviceConfigurationHandlers;
        private readonly List<Action<IServiceProvider, ILoggerFactory>> _configurationHandlers;
        private Func<IServiceCollection, ILoggerFactory, IServiceProvider> _serviceProviderBuilder;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MigratorHostBuilder()
        {
            _loggerFactory = new LoggerFactory();

            _loggingConfigurationHandlers = new List<Action<ILoggerFactory>>();
            _serviceConfigurationHandlers = new List<Action<IServiceCollection, ILoggerFactory>>();
            _configurationHandlers = new List<Action<IServiceProvider, ILoggerFactory>>();
            ServiceProviderBuilder = (services, factory) => services.BuildServiceProvider();
        }

        /// <inheritdoc />
        ~MigratorHostBuilder()
        {
            Dispose(false);
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Invoked when disposing the instance.
        /// </summary>
        /// <param name="disposing">True if disposing, otherwise false</param>
        protected virtual void Dispose(bool disposing)
        {
            if(_disposed)
                return;

            if (disposing)
                _loggerFactory?.Dispose();

            _loggerFactory = null;
            _loggingConfigurationHandlers.Clear();
            _serviceConfigurationHandlers.Clear();
            _configurationHandlers.Clear();
            _serviceProviderBuilder = null;

            _disposed = true;
        }

        #endregion

        #region Implementation of IMigratorHostBuilder

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ILoggerFactory>> LoggingConfigurationHandlers => _loggingConfigurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IServiceCollection, ILoggerFactory>> ServiceConfigurationHandlers => _serviceConfigurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IServiceProvider, ILoggerFactory>> ConfigurationHandlers => _configurationHandlers;

        /// <inheritdoc />
        public Func<IServiceCollection, ILoggerFactory, IServiceProvider> ServiceProviderBuilder
        {
            get { return _serviceProviderBuilder; }
            private set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _serviceProviderBuilder = value;
            }
        }

        /// <inheritdoc />
        public void AddLoggingConfigurator(Action<ILoggerFactory> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            _loggingConfigurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddServiceConfigurator(Action<IServiceCollection, ILoggerFactory> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _serviceConfigurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddConfigurator(Action<IServiceProvider, ILoggerFactory> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void SetServiceProviderBuilder(Func<IServiceCollection, ILoggerFactory, IServiceProvider> buildServiceProvider)
        {
            if (buildServiceProvider == null) throw new ArgumentNullException(nameof(buildServiceProvider));

            ServiceProviderBuilder = buildServiceProvider;
        }

        /// <inheritdoc />
        public void SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public IMigratorHost Build()
        {
            var loggerFactory = _loggerFactory;

            foreach (var handler in _loggingConfigurationHandlers)
                handler(loggerFactory);

            var logger = loggerFactory.CreateLogger<MigratorHostBuilder>();
            
            logger.LogDebug("Configuring core services");
            var serviceCollection = new ServiceCollection()
                .AddSingleton(loggerFactory)
                .AddLogging();

            if (_serviceConfigurationHandlers.Count == 0)
                logger.LogWarning("Service configuration handlers collection is empty. Host will only have access to core services...");
            else
            {
                logger.LogDebug(
                    "Configuring the host services using a total of {total} handlers",
                    _serviceConfigurationHandlers.Count);
                foreach (var handler in _serviceConfigurationHandlers)
                    handler(serviceCollection, loggerFactory);
            }

            logger.LogDebug("Building services provider");
            var serviceProvider = ServiceProviderBuilder(serviceCollection, loggerFactory);

            if (_configurationHandlers.Count == 0)
                logger.LogWarning("Configuration handlers collection is empty. Default configurations will be used...");
            else
            {
                logger.LogDebug(
                    "Configuring the host using a total of {total} handlers",
                    _configurationHandlers.Count);
                foreach (var handler in _configurationHandlers)
                    handler(serviceProvider, loggerFactory);
            }

            return new MigratorHost(serviceProvider, loggerFactory);
        }

        #endregion
    }
}