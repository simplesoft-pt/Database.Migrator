using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Hosting;
using SimpleSoft.Database.Migrator.Hosting.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests
{
    public class MigratorHostBuilderTests
    {
        [Fact]
        public void GivenAHostBuilderWhenBuildThenInvalidOperationExceptionMusBeThrown()
        {
            InvalidOperationException ex;

            using (var builder = new MigratorHostBuilder())
            {
                ex = Assert.Throws<InvalidOperationException>(() =>
                {
                    builder.Build<IMigrationContext>();
                });
            }

            Assert.NotNull(ex);
            Assert.True(ex.Message.IndexOf(nameof(IMigrationContext), StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [Fact]
        public void GivenAHostBuilderWithRegisteredMigrationManagerWhenBuildThenNoExceptionIsThrown()
        {
            using (var builder = new MigratorHostBuilder())
            {
                builder.AddServiceConfigurator(param =>
                {
                    param.ServiceCollection
                        .AddMigrationManager<TestMigrationManager, IMigrationContext>();
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);
            }
        }

        #region HostingEnvironment

        public void GivenAHostBuilderWhenAddingHostingEnvironmentConfiguratorThenParamMustNotBeNull()
        {
            IHostingEnvironment environment = null;
            using (var builder = new MigratorHostBuilder())
            {
                builder.AddHostingEnvironmentConfigurator(env =>
                {
                    environment = env;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(environment);
            }
        }

        public void GivenAHostBuilderWhenConfiguringHostingEnvironmentConfiguratorThenParamMustNotBeNull()
        {
            IHostingEnvironment environment = null;
            using (var builder = new MigratorHostBuilder()
                .ConfigureHostingEnvironment(env =>
                {
                    environment = env;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(environment);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenAddingMultipleHostingEnvironmentConfiguratorThenAllHandlersAreRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddHostingEnvironmentConfigurator(env =>
                {
                    ++runCount;
                });
                builder.AddHostingEnvironmentConfigurator(env =>
                {
                    ++runCount;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.HostingEnvironmentHandlers);
                Assert.Equal(2, builder.HostingEnvironmentHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenConfiguringMultipleHostingEnvironmentConfiguratorThenAllHandlersAreRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder()
                .ConfigureHostingEnvironment(param =>
                {
                    ++runCount;
                })
                .ConfigureHostingEnvironment(param =>
                {
                    ++runCount;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.HostingEnvironmentHandlers);
                Assert.Equal(2, builder.HostingEnvironmentHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenRunningTheHostingConfiguratorThenAllPropertiesMustBeAssigned()
        {
            IHostingEnvironment environment = null;
            using (var builder = new MigratorHostBuilder())
            {
                builder.AddHostingEnvironmentConfigurator(env =>
                {
                    environment = env;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(environment);

                Assert.NotNull(environment.ApplicationName);
                Assert.False(string.IsNullOrWhiteSpace(environment.ApplicationName));

                Assert.NotNull(environment.Name);
                Assert.False(string.IsNullOrWhiteSpace(environment.Name));

                Assert.NotNull(environment.ContentRootPath);
                Assert.False(string.IsNullOrWhiteSpace(environment.ContentRootPath));

                Assert.NotNull(environment.ContentRootFileProvider);
            }
        }

        #endregion

        #region ConfigurationBuilder

        [Fact]
        public void GivenAHostBuilderWhenAddingConfigurationBuilderConfiguratorThenParamMustNotBeNull()
        {
            ConfigurationBuilderConfiguratorParam parameter = null;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddConfigurationBuilderConfigurator(param =>
                {
                    parameter = param;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(parameter);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenConfiguringConfigurationBuildersThenParamMustNotBeNull()
        {
            ConfigurationBuilderConfiguratorParam parameter = null;

            using (var builder = new MigratorHostBuilder()
                .ConfigureConfigurationBuilder(param =>
                {
                    parameter = param;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(parameter);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenAddingMultipleConfigurationBuilderConfiguratorThenAllHandlersAreRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddConfigurationBuilderConfigurator(param =>
                {
                    ++runCount;
                });
                builder.AddConfigurationBuilderConfigurator(param =>
                {
                    ++runCount;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.ConfigurationBuilderHandlers);
                Assert.Equal(2, builder.ConfigurationBuilderHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenConfiguringMultipleConfigurationBuildersThenAllHandlersAreRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder()
                .ConfigureConfigurationBuilder(param =>
                {
                    ++runCount;
                })
                .ConfigureConfigurationBuilder(param =>
                {
                    ++runCount;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.ConfigurationBuilderHandlers);
                Assert.Equal(2, builder.ConfigurationBuilderHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        #endregion

        #region Configuration

        [Fact]
        public void GivenAHostBuilderWhenAddingConfigurationConfiguratorThenParamMustNotBeNull()
        {
            ConfigurationConfiguratorParam parameter = null;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddConfigurationConfigurator(param =>
                {
                    parameter = param;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);
            }

            Assert.NotNull(parameter);
        }

        [Fact]
        public void GivenAHostBuilderWhenConfiguringConfigurationsThenParamMustNotBeNull()
        {
            ConfigurationConfiguratorParam parameter = null;

            using (var builder = new MigratorHostBuilder()
                .ConfigureConfigurations(param =>
                {
                    parameter = param;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);
            }

            Assert.NotNull(parameter);
        }

        [Fact]
        public void GivenAHostBuilderWhenAddingMultipleConfigurationConfiguratorThenAllHandlersAreRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddConfigurationConfigurator(param =>
                {
                    ++runCount;
                });
                builder.AddConfigurationConfigurator(param =>
                {
                    ++runCount;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.ConfigurationHandlers);
                Assert.Equal(2, builder.ConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenConfiguringMultipleConfigurationsThenAllHandlersAreRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder()
                .ConfigureConfigurations(param =>
                {
                    ++runCount;
                })
                .ConfigureConfigurations(param =>
                {
                    ++runCount;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.ConfigurationHandlers);
                Assert.Equal(2, builder.ConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        #endregion

        #region LoggerFactory
        
        [Fact]
        public void GivenAHostBuilderWhenBuildingThenDefaultLoggerFactoryMustNotBeNull()
        {
            ILoggerFactory builderFactory = null;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddLoggingConfigurator(param =>
                {
                    builderFactory = param.Factory;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);
            }

            Assert.NotNull(builderFactory);
        }
        
        [Fact]
        public void GivenAHostBuilderWhenSettingACustomFactoryThenItMustBeUsed()
        {
            ILoggerFactory builderFactory = null;
            var originalFactory = new LoggerFactory();

            using (var builder = new MigratorHostBuilder())
            {
                builder.SetLoggerFactory(originalFactory);
                builder.AddLoggingConfigurator(param =>
                {
                    builderFactory = param.Factory;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(builderFactory);
                Assert.Same(originalFactory, builderFactory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenUsingCustomFactoryThenItMustBeUsed()
        {
            ILoggerFactory builderFactory = null;
            var originalFactory = new LoggerFactory();

            using (var builder = new MigratorHostBuilder()
                .UseLoggerFactory(originalFactory))
            {
                builder.AddLoggingConfigurator(param =>
                {
                    builderFactory = param.Factory;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(builderFactory);
                Assert.Same(originalFactory, builderFactory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenSettingNullFactoryThenArgumentNullExceptionMustBeThrown()
        {
            using (var builder = new MigratorHostBuilder())
            {
                var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    builder.SetLoggerFactory(null);
                });

                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenUsingNullFactoryThenArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                using (new MigratorHostBuilder()
                    .UseLoggerFactory(null))
                {

                }
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void GivenAHostBuilderWhenAddingLoggingConfiguratorsThenAllHandlersMustBeRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddLoggingConfigurator(param =>
                {
                    ++runCount;
                });
                builder.AddLoggingConfigurator(param =>
                {
                    ++runCount;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.LoggingConfigurationHandlers);
                Assert.Equal(2, builder.LoggingConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenConfigureLoggingThenAllHandlersMustBeRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder()
                .ConfigureLogging(f =>
                {
                    ++runCount;
                })
                .ConfigureLogging(f =>
                {
                    ++runCount;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.LoggingConfigurationHandlers);
                Assert.Equal(2, builder.LoggingConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        #endregion

        #region ServiceCollection

        [Fact]
        public void GivenAHostBuilderWhenAddingServiceConfiguratorThenParametersMustNotBeNull()
        {
            IServiceCollection services = null;
            ILoggerFactory factory = null;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddServiceConfigurator(param =>
                {
                    services = param.ServiceCollection;
                    factory = param.Factory;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(services);
                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenAddingServiceConfiguratorsThenAllHandlersMustBeRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddServiceConfigurator(param =>
                {
                    ++runCount;
                });
                builder.AddServiceConfigurator(param =>
                {
                    ++runCount;
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.ServiceConfigurationHandlers);
                Assert.Equal(2, builder.ServiceConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenConfiguringServicesThenAllHandlersMustBeRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder()
                .ConfigureServices(param =>
                {
                    ++runCount;
                })
                .ConfigureServices(param =>
                {
                    ++runCount;
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotEmpty(builder.ServiceConfigurationHandlers);
                Assert.Equal(2, builder.ServiceConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        #endregion

        #region ServiceProvider

        [Fact]
        public void GivenAHostBuilderWhenBuildingServiceProviderThenParametersMustNotBeNull()
        {
            IServiceCollection services = null;
            ILoggerFactory factory = null;

            using (var builder = new MigratorHostBuilder())
            {
                builder.SetServiceProviderBuilder(param =>
                {
                    services = param.ServiceCollection;
                    factory = param.Factory;

                    return param.ServiceCollection.BuildServiceProvider();
                });
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(services);
                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenUsingServiceProviderThenParametersMustNotBeNull()
        {
            IServiceCollection services = null;
            ILoggerFactory factory = null;

            using (var builder = new MigratorHostBuilder()
                .UseServiceProvider(param =>
                {
                    services = param.ServiceCollection;
                    factory = param.Factory;

                    return param.ServiceCollection.BuildServiceProvider();
                }))
            {
                BuildAndIgnoreMissingMigrationManagerException(builder);

                Assert.NotNull(services);
                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenPassingNullServiceProviderBuilderThenArgumentNullExceptionMustBeThrown()
        {
            using (var builder = new MigratorHostBuilder())
            {
                var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    builder.SetServiceProviderBuilder(null);
                    builder.Build<IMigrationContext>();
                });

                Assert.NotNull(ex);
            }
        }

        #endregion

        #region Dispose

        [Fact]
        public void GivenAHostBuilderWhenBuildingAfterDisposedThenObjectDisposedExceptionMustBeThrown()
        {
            var builder = new MigratorHostBuilder();
            builder.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() =>
            {
                builder.Build<IMigrationContext>();
            });

            Assert.NotNull(ex);
        }

        #endregion

        private static void BuildAndIgnoreMissingMigrationManagerException(IMigratorHostBuilder builder)
        {
            try
            {
                builder.Build<IMigrationContext>();
            }
            catch (InvalidOperationException)
            {
                //  ignoring exception due to missing registration of
                //  IMigrationManager<IMigrationContext>. Test porpuses only
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestMigrationManager : IMigrationManager<IMigrationContext>
        {
            #region Implementation of IMigrationManager<out IMigrationContext>

            /// <inheritdoc />
            public IMigrationContext Context { get; } = null;

            /// <inheritdoc />
            public string ContextName { get; } = null;

            /// <inheritdoc />
            public Task PrepareDatabaseAsync(CancellationToken ct)
            {
                return Task.CompletedTask;
            }

            /// <inheritdoc />
            public Task AddMigrationAsync(string migrationId, string className, string description, CancellationToken ct)
            {
                return Task.CompletedTask;
            }

            /// <inheritdoc />
            public Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(CancellationToken ct)
            {
                return Task.FromResult<IReadOnlyCollection<string>>(new string[0]);
            }

            /// <inheritdoc />
            public Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct)
            {
                return Task.FromResult(string.Empty);
            }

            /// <inheritdoc />
            public Task<bool> RemoveMostRecentMigrationAsync(CancellationToken ct)
            {
                return Task.FromResult(false);
            }

            #endregion
        }
    }
}
