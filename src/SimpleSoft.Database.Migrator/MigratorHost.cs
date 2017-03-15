using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Represents a configured migrator host
    /// </summary>
    public class MigratorHost : IMigratorHost
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MigratorHost> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="logger">The logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MigratorHost(IServiceProvider serviceProvider, ILogger<MigratorHost> logger)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _serviceProvider = serviceProvider;
            _logger = logger;
        }
    }
}
