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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Handlers;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extension methods for <see cref="IMigratorHostBuilder"/>
    /// </summary>
    public static class MigratorHostBuilderExtensions
    {
        #region NamingNormalizer

        /// <summary>
        /// Assigns the given <see cref="INamingNormalizer"/> to be used
        /// by the <see cref="IMigratorHost{TContext}"/>.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="normalizer">The normalizer to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseNamingNormalizer<TBuilder>(this TBuilder builder, INamingNormalizer normalizer)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.NamingNormalizer = normalizer;
            return builder;
        }

        /// <summary>
        /// Assigns an instance of <see cref="DefaultNamingNormalizer"/> to be used
        /// by the <see cref="IMigratorHost{TContext}"/>.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseDefaultNamingNormalizer<TBuilder>(this TBuilder builder)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.NamingNormalizer = new DefaultNamingNormalizer();
            return builder;
        }

        /// <summary>
        /// Assigns an instance of <see cref="TrimNamingNormalizer"/> to be used
        /// by the <see cref="IMigratorHost{TContext}"/>.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseTrimNamingNormalizer<TBuilder>(this TBuilder builder)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.NamingNormalizer = new TrimNamingNormalizer();
            return builder;
        }

        #endregion

        /// <summary>
        /// Adds the handler to the <see cref="IMigratorHostBuilder.HostingEnvironmentHandlers"/> 
        /// collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureHostingEnvironment<TBuilder>(this TBuilder builder, Action<IHostingEnvironment> handler)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddHostingEnvironmentConfigurator(handler);
            return builder;
        }

        /// <summary>
        /// Adds the handler to the <see cref="IMigratorHostBuilder.ConfigurationBuilderHandlers"/> 
        /// collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfigurationBuilder<TBuilder>(this TBuilder builder, Action<ConfigurationBuilderConfiguratorParam> handler)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurationBuilderConfigurator(handler);
            return builder;
        }

        /// <summary>
        /// Adds the handler to the <see cref="IMigratorHostBuilder.ConfigurationHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfigurations<TBuilder>(this TBuilder builder, Action<ConfigurationConfiguratorParam> handler)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurationConfigurator(handler);
            return builder;
        }

        /// <summary>
        /// Adds the handler to the <see cref="IMigratorHostBuilder.LoggingConfigurationHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureLogging<TBuilder>(this TBuilder builder, Action<LoggingConfiguratorParam> handler)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddLoggingConfigurator(handler);
            return builder;
        }

        /// <summary>
        /// Adds the handler to the <see cref="IMigratorHostBuilder.ServiceConfigurationHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureServices<TBuilder>(this TBuilder builder, Action<ServiceConfiguratorParam> handler)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddServiceConfigurator(handler);
            return builder;
        }

        /// <summary>
        /// Uses the given handler to build the <see cref="IServiceProvider"/> that
        /// will be used by the <see cref="IMigratorHost{TContext}"/> to build.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="buildServiceProvider">The builder function</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseServiceProvider<TBuilder>(this TBuilder builder, Func<ServiceProviderBuilderParam, IServiceProvider> buildServiceProvider)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.SetServiceProviderBuilder(buildServiceProvider);
            return builder;
        }

        /// <summary>
        /// Adds the handler to the <see cref="IMigratorHostBuilder.ConfigureHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder Configure<TBuilder>(this TBuilder builder, Action<ConfigureParam> handler)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurator(handler);
            return builder;
        }

        /// <summary>
        /// Assigns the given <see cref="ILoggerFactory"/> to be used
        /// by the <see cref="IMigratorHost{TContext}"/>.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="loggerFactory">The logger factory to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseLoggerFactory<TBuilder>(this TBuilder builder, ILoggerFactory loggerFactory)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.SetLoggerFactory(loggerFactory);
            return builder;
        }

        /// <summary>
        /// Appends all the given configuration entries into the one used by the migrator builder.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="configuration">The configuration to append</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseConfiguration<TBuilder>(this TBuilder builder, IConfiguration configuration)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.AddConfigurationConfigurator(param =>
            {
                foreach (var values in configuration.AsEnumerable())
                    param.Configuration[values.Key] = values.Value;
            });
            return builder;
        }

        #region Use

        public static IMigratorHostBuilder Use<TStartup>(this IMigratorHostBuilder builder)
            where TStartup : IStartup, new()
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder;
        }

        /// <summary>
        /// Uses the given class to be used as the startup configurator for the host builder
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder instance</param>
        /// <param name="startup">The startup instance</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder Use<TBuilder>(this TBuilder builder, IStartup startup)
            where TBuilder : IMigratorHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (startup == null) throw new ArgumentNullException(nameof(startup));

            builder.AddHostingEnvironmentConfigurator(startup.ConfigureHostingEnvironment);
            builder.AddConfigurationBuilderConfigurator(startup.ConfigureConfigurationBuilder);
            builder.AddConfigurationConfigurator(startup.ConfigureConfigurations);
            builder.AddLoggingConfigurator(startup.ConfigureLogging);
            builder.AddServiceConfigurator(startup.ConfigureServices);
            builder.SetServiceProviderBuilder(startup.BuildServiceProvider);
            builder.AddConfigurator(startup.Configure);

            return builder;
        }

        #endregion
    }
}