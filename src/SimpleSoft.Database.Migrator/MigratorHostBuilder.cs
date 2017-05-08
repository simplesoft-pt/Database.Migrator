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
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Handlers;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The <see cref="IMigratorHost{TContext}"/> builder
    /// </summary>
    public class MigratorHostBuilder : IMigratorHostBuilder, IDisposable
    {
        private ILoggerFactory _loggerFactory;
        private bool _disposed;
        private INamingNormalizer _namingNormalizer;
        private readonly List<Action<ConfigurationBuilderConfiguratorParam>> _configurationBuilderHandlers;
        private readonly List<Action<ConfigurationConfiguratorParam>> _configurationHandlers;
        private readonly List<Action<LoggingConfiguratorParam>> _loggingConfigurationHandlers;
        private readonly List<Action<ServiceConfiguratorParam>> _serviceConfigurationHandlers;
        private readonly List<Action<ConfigureParam>> _configureHandlers;
        private Func<ServiceProviderBuilderParam, IServiceProvider> _serviceProviderBuilder;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MigratorHostBuilder()
        {
            _loggerFactory = new LoggerFactory();
            _namingNormalizer = new DefaultNamingNormalizer();

            _configurationBuilderHandlers = new List<Action<ConfigurationBuilderConfiguratorParam>>();
            _configurationHandlers = new List<Action<ConfigurationConfiguratorParam>>();
            _loggingConfigurationHandlers = new List<Action<LoggingConfiguratorParam>>();
            _serviceConfigurationHandlers = new List<Action<ServiceConfiguratorParam>>();
            _configureHandlers = new List<Action<ConfigureParam>>();
            _serviceProviderBuilder = args => args.ServiceCollection.BuildServiceProvider();
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
            _namingNormalizer = null;
            _configurationBuilderHandlers.Clear();
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
        public INamingNormalizer NamingNormalizer
        {
            get { return _namingNormalizer; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _namingNormalizer = value;
            }
        }

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigurationBuilderConfiguratorParam>> ConfigurationBuilderHandlers => _configurationBuilderHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigurationConfiguratorParam>> ConfigurationHandlers => _configurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<LoggingConfiguratorParam>> LoggingConfigurationHandlers => _loggingConfigurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ServiceConfiguratorParam>> ServiceConfigurationHandlers => _serviceConfigurationHandlers;

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigureParam>> ConfigureHandlers => _configureHandlers;

        /// <inheritdoc />
        public Func<ServiceProviderBuilderParam, IServiceProvider> ServiceProviderBuilder
        {
            get { return _serviceProviderBuilder; }
            private set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _serviceProviderBuilder = value;
            }
        }

        /// <inheritdoc />
        public void AddConfigurationBuilderConfigurator(Action<ConfigurationBuilderConfiguratorParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationBuilderHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddConfigurationConfigurator(Action<ConfigurationConfiguratorParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddLoggingConfigurator(Action<LoggingConfiguratorParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            _loggingConfigurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddServiceConfigurator(Action<ServiceConfiguratorParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _serviceConfigurationHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void AddConfigurator(Action<ConfigureParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configureHandlers.Add(handler);
        }

        /// <inheritdoc />
        public void SetServiceProviderBuilder(Func<ServiceProviderBuilderParam, IServiceProvider> buildServiceProvider)
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
        public IMigratorHost<TContext> Build<TContext>() 
            where TContext : IMigrationContext
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MigratorHostBuilder));

            var cfgBuilderParam = new ConfigurationBuilderConfiguratorParam(
                new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {MigratorHostDefaults.EnvironmentKey, MigratorHostDefaults.DefaultEnvironment},
                        {MigratorHostDefaults.ContentRootKey, Directory.GetCurrentDirectory()}
                    })
                    .AddEnvironmentVariables("DATABASE_MIGRATOR_"));
            foreach (var handler in _configurationBuilderHandlers)
                handler(cfgBuilderParam);
            
            var cfgParam = new ConfigurationConfiguratorParam(cfgBuilderParam.Builder.Build());
            foreach (var handler in _configurationHandlers)
                handler(cfgParam);

            var configuration = cfgParam.Configuration;
            var loggerFactory = _loggerFactory;
            var loggingParam = new LoggingConfiguratorParam(loggerFactory, configuration);
            foreach (var handler in _loggingConfigurationHandlers)
                handler(loggingParam);

            var logger = loggerFactory.CreateLogger<MigratorHostBuilder>();
            
            logger.LogDebug("Configuring core services");
            var serviceCollection = new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton(loggerFactory)
                .AddSingleton(_namingNormalizer)
                .AddLogging();

            if (_serviceConfigurationHandlers.Count == 0)
                logger.LogWarning("Service configuration handlers collection is empty. Host will only have access to core services...");
            else
            {
                logger.LogDebug(
                    "Configuring the host services using a total of {total} handlers",
                    _serviceConfigurationHandlers.Count);
                var serviceParam =
                    new ServiceConfiguratorParam(serviceCollection, loggerFactory, configuration);
                foreach (var handler in _serviceConfigurationHandlers)
                    handler(serviceParam);
            }

            logger.LogDebug("Building services provider");
            var serviceProvider = ServiceProviderBuilder(new ServiceProviderBuilderParam(
                serviceCollection, loggerFactory, configuration));

            if (_configureHandlers.Count == 0)
                logger.LogWarning("Configuration handlers collection is empty. Default configurations will be used...");
            else
            {
                logger.LogDebug(
                    "Configuring the host using a total of {total} handlers",
                    _configureHandlers.Count);
                var configureParam = new ConfigureParam(serviceProvider, loggerFactory, configuration);
                foreach (var handler in _configureHandlers)
                    handler(configureParam);
            }

            var migrationInterfaceType = typeof(IMigration<TContext>).GetTypeInfo();

            var migrationImplTypes = new List<Type>(serviceCollection.Count);
            migrationImplTypes.AddRange(
                serviceCollection.Where(e => migrationInterfaceType.IsAssignableFrom(e.ImplementationType))
                    .Select(e => e.ImplementationType));

            if (migrationImplTypes.Count == 0)
            {
                if (logger.IsEnabled(LogLevel.Warning))
                    logger.LogWarning(
                        "No migrations were found for the context '{contextType}'", typeof(TContext).Name);
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug(
                        "A total of {totalMigrations} migrations were found for the context '{contextType}'",
                        migrationImplTypes.Count, typeof(TContext).Name);
            }

            return new MigratorHost<TContext>(
                serviceProvider, configuration,
                serviceProvider.GetRequiredService<IMigrationManager<TContext>>(),
                migrationImplTypes, loggerFactory.CreateLogger<MigratorHost<TContext>>());
        }

        #endregion
    }
}