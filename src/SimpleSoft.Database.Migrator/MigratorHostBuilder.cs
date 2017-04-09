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
    public class MigratorHostBuilder : IMigratorHostBuilder, IDisposable
    {
        private IConfigurationRoot _configuration;
        private ILoggerFactory _loggerFactory;
        private bool _disposed;
        private readonly List<Action<IConfiguration>> _configurationHandlers;
        private readonly List<Action<ILoggerFactory, IConfiguration>> _loggingConfigurationHandlers;
        private readonly List<Action<IServiceCollection, ILoggerFactory, IConfiguration>> _serviceConfigurationHandlers;
        private readonly List<Action<IServiceProvider, ILoggerFactory, IConfiguration>> _configureHandlers;
        private Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> _serviceProviderBuilder;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MigratorHostBuilder()
        {
            _configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables("DATABASE_MIGRATOR_")
                .Build();

            if (string.IsNullOrWhiteSpace(this.GetEnvironment()))
                this.UseEnvironment(MigratorHostDefaults.DefaultEnvironment);

            _loggerFactory = new LoggerFactory();

            _configurationHandlers = new List<Action<IConfiguration>>();
            _loggingConfigurationHandlers = new List<Action<ILoggerFactory, IConfiguration>>();
            _serviceConfigurationHandlers = new List<Action<IServiceCollection, ILoggerFactory, IConfiguration>>();
            _configureHandlers = new List<Action<IServiceProvider, ILoggerFactory, IConfiguration>>();
            ServiceProviderBuilder = (services, factory, config) => services.BuildServiceProvider();
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

            _configuration = null;
            _loggerFactory = null;
            _configurationHandlers.Clear();
            _loggingConfigurationHandlers.Clear();
            _serviceConfigurationHandlers.Clear();
            _configureHandlers.Clear();
            _serviceProviderBuilder = null;

            _disposed = true;
        }

        #endregion

        #region Implementation of IMigratorHostBuilder

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IConfiguration>> ConfigurationHandlers => _configurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ILoggerFactory, IConfiguration>> LoggingConfigurationHandlers => _loggingConfigurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IServiceCollection, ILoggerFactory, IConfiguration>> ServiceConfigurationHandlers => _serviceConfigurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IServiceProvider, ILoggerFactory, IConfiguration>> ConfigureHandlers => _configureHandlers;

        /// <inheritdoc />
        public Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> ServiceProviderBuilder
        {
            get { return _serviceProviderBuilder; }
            private set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _serviceProviderBuilder = value;
            }
        }

        /// <inheritdoc />
        public void AddConfigurationConfigurator(Action<IConfiguration> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddLoggingConfigurator(Action<ILoggerFactory, IConfiguration> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            _loggingConfigurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddServiceConfigurator(Action<IServiceCollection, ILoggerFactory, IConfiguration> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _serviceConfigurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddConfigurator(Action<IServiceProvider, ILoggerFactory, IConfiguration> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configureHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void SetServiceProviderBuilder(Func<IServiceCollection, ILoggerFactory, IConfiguration, IServiceProvider> buildServiceProvider)
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
        public string GetSetting(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be whitespace.", nameof(key));

            return _configuration[key];
        }

        /// <inheritdoc />
        public void SetSetting(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be whitespace.", nameof(key));

            _configuration[key] = value;
        }

        /// <inheritdoc />
        public IMigratorHost Build()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MigratorHostBuilder));

            var configuration = _configuration;
            foreach (var handler in _configurationHandlers)
                handler(configuration);

            var loggerFactory = _loggerFactory;
            foreach (var handler in _loggingConfigurationHandlers)
                handler(loggerFactory, configuration);

            var logger = loggerFactory.CreateLogger<MigratorHostBuilder>();
            
            logger.LogDebug("Configuring core services");
            var serviceCollection = new ServiceCollection()
                .AddSingleton(configuration)
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
                    handler(serviceCollection, loggerFactory, configuration);
            }

            logger.LogDebug("Building services provider");
            var serviceProvider = ServiceProviderBuilder(serviceCollection, loggerFactory, configuration);

            if (_configureHandlers.Count == 0)
                logger.LogWarning("Configuration handlers collection is empty. Default configurations will be used...");
            else
            {
                logger.LogDebug(
                    "Configuring the host using a total of {total} handlers",
                    _configureHandlers.Count);
                foreach (var handler in _configureHandlers)
                    handler(serviceProvider, loggerFactory, configuration);
            }

            return new MigratorHost(serviceProvider, loggerFactory, configuration);
        }

        #endregion
    }
}