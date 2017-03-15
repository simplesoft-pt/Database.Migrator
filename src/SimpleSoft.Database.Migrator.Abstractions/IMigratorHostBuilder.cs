using System;
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
        /// Uses the given handler to configure the <see cref="ILoggerFactory"/>
        /// used by the <see cref="IMigratorHost"/> to build.
        /// </summary>
        /// <param name="configureLogging">The configuration handler</param>
        /// <returns>The <see cref="IMigratorHostBuilder"/></returns>
        IMigratorHostBuilder ConfigureLogging(Action<ILoggerFactory> configureLogging);

        /// <summary>
        /// Uses the given handler to add services to the <see cref="IServiceCollection"/>
        /// used by the <see cref="IMigratorHost"/> to build.
        /// </summary>
        /// <param name="configureServices">The configuration handler</param>
        /// <returns>The <see cref="IMigratorHostBuilder"/></returns>
        IMigratorHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// Uses the given handler to build the <see cref="IServiceProvider"/> that
        /// will be used by the <see cref="IMigratorHost"/> to build.
        /// </summary>
        /// <param name="configureServiceProvider">The configuration handler</param>
        /// <returns>The <see cref="IMigratorHostBuilder"/></returns>
        IMigratorHostBuilder ConfigureServiceProvider(Func<IServiceCollection, IServiceProvider> configureServiceProvider);

        /// <summary>
        /// Assigns the given <see cref="ILoggerFactory"/> to be used
        /// by the <see cref="IMigratorHost"/>. If an handler was specified by <see cref="ConfigureLogging"/>,
        /// it will be invoked with this instance as a parameter.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use</param>
        /// <returns>The <see cref="IMigratorHostBuilder"/></returns>
        IMigratorHostBuilder UseLoggerFactory(ILoggerFactory loggerFactory);

        /// <summary>
        /// Builds an instance of <see cref="IMigratorHost"/> to run migrations.
        /// </summary>
        /// <returns>The <see cref="IMigratorHost"/> instance</returns>
        IMigratorHost Build();
    }
}
