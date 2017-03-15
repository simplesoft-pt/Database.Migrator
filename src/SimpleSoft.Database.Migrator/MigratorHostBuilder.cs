using System;
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
        private Action<ILoggerFactory> _configureLogging;
        private Action<IServiceCollection> _configureServices;
        private Action<IServiceProvider, ILoggerFactory> _configure;
        private Func<IServiceCollection, IServiceProvider> _buildServiceProvider;
        private bool _disposed;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MigratorHostBuilder()
        {
            _loggerFactory = new LoggerFactory();
        }

        /// <inheritdoc />
        ~MigratorHostBuilder()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public IMigratorHostBuilder ConfigureLogging(Action<ILoggerFactory> configureLogging)
        {
            if (configureLogging == null)
                throw new ArgumentNullException(nameof(configureLogging));

            _configureLogging = configureLogging;
            return this;
        }

        /// <inheritdoc />
        public IMigratorHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices == null)
                throw new ArgumentNullException(nameof(configureServices));

            _configureServices = configureServices;
            return this;
        }

        /// <inheritdoc />
        public IMigratorHostBuilder Configure(Action<IServiceProvider, ILoggerFactory> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            _configure = configure;
            return this;
        }

        /// <inheritdoc />
        public IMigratorHostBuilder UseServiceProvider(Func<IServiceCollection, IServiceProvider> buildServiceProvider)
        {
            if (buildServiceProvider == null)
                throw new ArgumentNullException(nameof(buildServiceProvider));

            _buildServiceProvider = buildServiceProvider;
            return this;
        }

        /// <inheritdoc />
        public IMigratorHostBuilder UseLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _loggerFactory = loggerFactory;
            return this;
        }

        /// <inheritdoc />
        public IMigratorHost Build()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MigratorHostBuilder));

            _configureLogging?.Invoke(_loggerFactory);

            var logger = _loggerFactory.CreateLogger<MigratorHostBuilder>();

            logger.LogDebug("Configuring the service collection");

            var serviceCollection =
                new ServiceCollection()
                    .AddSingleton(_loggerFactory)
                    .AddLogging();
            _configureServices?.Invoke(serviceCollection);

            var serviceProvider =
                _buildServiceProvider == null
                    ? serviceCollection.BuildServiceProvider()
                    : _buildServiceProvider(serviceCollection);

            _configure?.Invoke(serviceProvider, _loggerFactory);

            return new MigratorHost(serviceProvider, _loggerFactory.CreateLogger<MigratorHost>());
        }

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
            _configureLogging = null;
            _configureServices = null;
            _configure = null;
            _buildServiceProvider = null;

            _disposed = true;
        }
    }
}