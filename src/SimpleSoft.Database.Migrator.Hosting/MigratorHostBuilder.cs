﻿#region License
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
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Hosting.Handlers;

namespace SimpleSoft.Database.Migrator.Hosting
{
    /// <summary>
    /// The <see cref="IMigratorHost{TContext}"/> builder
    /// </summary>
    public class MigratorHostBuilder : IMigratorHostBuilder, IDisposable
    {
        private ILoggerFactory _loggerFactory;
        private bool _disposed;
        private INamingNormalizer _namingNormalizer;
        private readonly List<Action<IHostingEnvironment>> _hostingEnvironmentHandlers;
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

            _hostingEnvironmentHandlers = new List<Action<IHostingEnvironment>>();
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
            _hostingEnvironmentHandlers.Clear();
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
        public IReadOnlyCollection<Action<IHostingEnvironment>> HostingEnvironmentHandlers => _hostingEnvironmentHandlers;

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
        public void AddHostingEnvironmentConfigurator(Action<IHostingEnvironment> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _hostingEnvironmentHandlers.Add(handler);
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

            var environment = BuildHostingEnvironment();

            var configurationBuilder = BuildConfigurationBuilderUsingHandlers(environment);

            var configuration = 
                BuidConfigurationRootUsingHandlers(configurationBuilder, environment);

            var loggerFactory = 
                BuildLoggerFactoryUsingHandlers(configuration, environment);

            var logger = loggerFactory.CreateLogger<MigratorHostBuilder>();
            var serviceCollection =
                BuildServiceCollectionUsingHandlers(logger, environment, configuration, loggerFactory, environment);

            var serviceProvider =
                BuildServiceProviderUsingHandlers(logger, serviceCollection, configuration, loggerFactory, environment);

            FailIfNoContextExist<TContext>(serviceCollection);

            var migrationImplTypes =
                ExtractMigrationMetadatas<TContext>(logger, _namingNormalizer, serviceCollection);

            return new MigratorHost<TContext>(
                serviceProvider, _namingNormalizer, migrationImplTypes,
                loggerFactory.CreateLogger<MigratorHost<TContext>>());
        }

        #endregion

        #region Private

        private IHostingEnvironment BuildHostingEnvironment()
        {
            var contentRootPath = Directory.GetCurrentDirectory();

            var environment =
                Environment.GetEnvironmentVariable(
                    "DATABASE_MIGRATOR_" + MigratorHostDefaults.EnvironmentKey);
            if (string.IsNullOrWhiteSpace(environment))
                environment = MigratorHostDefaults.DefaultEnvironment;

            var hosting = new HostingEnvironment
            {
                ApplicationName = Assembly.GetEntryAssembly().GetName().Name,
                ContentRootPath = contentRootPath,
                ContentRootFileProvider = new PhysicalFileProvider(contentRootPath),
                Name = environment
            };

            foreach (var handler in _hostingEnvironmentHandlers)
                handler(hosting);

            return hosting;
        }

        private IConfigurationBuilder BuildConfigurationBuilderUsingHandlers(IHostingEnvironment environment)
        {
            var cfgBuilderParam = new ConfigurationBuilderConfiguratorParam(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {MigratorHostDefaults.EnvironmentKey, environment.Name},
                    {MigratorHostDefaults.ContentRootKey, environment.ContentRootPath}
                }), environment);
            foreach (var handler in _configurationBuilderHandlers)
                handler(cfgBuilderParam);

            return cfgBuilderParam.Builder;
        }

        private IConfigurationRoot BuidConfigurationRootUsingHandlers(
            IConfigurationBuilder configurationBuilder, IHostingEnvironment environment)
        {
            var cfgParam = new ConfigurationConfiguratorParam(configurationBuilder.Build(), environment);
            foreach (var handler in _configurationHandlers)
                handler(cfgParam);

            return cfgParam.Configuration;
        }

        private ILoggerFactory BuildLoggerFactoryUsingHandlers(IConfiguration configuration, IHostingEnvironment environment)
        {
            var loggerFactory = _loggerFactory;
            var loggingParam = new LoggingConfiguratorParam(loggerFactory, configuration, environment);
            foreach (var handler in _loggingConfigurationHandlers)
                handler(loggingParam);

            return loggerFactory;
        }

        private IServiceCollection BuildServiceCollectionUsingHandlers(
            ILogger logger, IHostingEnvironment hosting, IConfigurationRoot configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            logger.LogDebug("Configuring core services");
            var serviceCollection = new ServiceCollection()
                .AddSingleton(hosting)
                .AddSingleton(hosting.ContentRootFileProvider)
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
                    new ServiceConfiguratorParam(serviceCollection, loggerFactory, configuration, environment);
                foreach (var handler in _serviceConfigurationHandlers)
                    handler(serviceParam);
            }

            return serviceCollection;
        }

        private IServiceProvider BuildServiceProviderUsingHandlers(
            ILogger logger, IServiceCollection serviceCollection, IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            logger.LogDebug("Building services provider");
            var serviceProvider = ServiceProviderBuilder(new ServiceProviderBuilderParam(
                serviceCollection, loggerFactory, configuration, environment));

            if (_configureHandlers.Count == 0)
                logger.LogWarning("Configuration handlers collection is empty. Default configurations will be used...");
            else
            {
                logger.LogDebug(
                    "Configuring the host using a total of {total} handlers",
                    _configureHandlers.Count);
                var configureParam = new ConfigureParam(serviceProvider, loggerFactory, configuration, environment);
                foreach (var handler in _configureHandlers)
                    handler(configureParam);
            }

            return serviceProvider;
        }

        private static void FailIfNoContextExist<TContext>(IServiceCollection serviceCollection)
            where TContext : IMigrationContext
        {
            var contextType = typeof(TContext);

            var context = serviceCollection.SingleOrDefault(e => contextType == e.ServiceType);
            if (context == null)
                throw new InvalidOperationException(
                    $"No context of type '{contextType.FullName}' was found in the service collection.");
        }

        private static IEnumerable<MigrationMetadata<TContext>> ExtractMigrationMetadatas<TContext>(
            ILogger logger, INamingNormalizer namingNormalizer, IServiceCollection serviceCollection)
            where TContext : IMigrationContext
        {
            logger.LogDebug("Searching for registered migrations");

            var migrationInterfaceType = typeof(IMigration<TContext>).GetTypeInfo();

            var migrationImplTypes = new List<MigrationMetadata<TContext>>(serviceCollection.Count);
            migrationImplTypes.AddRange(
                serviceCollection.Where(e =>
                    {
                        if (migrationInterfaceType.IsAssignableFrom(e.ServiceType))
                        {
                            var type = e.ServiceType.GetTypeInfo();
                            return type.IsClass && !type.IsAbstract;
                        }
                        return false;
                    })
                    .Select(e => new MigrationMetadata<TContext>(
                        namingNormalizer.Normalize(e.ImplementationType.Name),
                        namingNormalizer.Normalize(e.ImplementationType.FullName),
                        e.ImplementationType)));

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

            return migrationImplTypes;
        }

        #endregion
    }
}