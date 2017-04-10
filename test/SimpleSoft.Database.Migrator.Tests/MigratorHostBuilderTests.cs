using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests
{
    public class MigratorHostBuilderTests
    {
        #region LoggerFactory

        [Fact]
        public void GivenAHostBuilderWithDefaultValuesThenLoggerFactoryMustNotBeNull()
        {
            ILoggerFactory factory = null;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddLoggingConfigurator((f, cfg) =>
                {
                    factory = f;
                });
                builder.Build<IMigrationContext>();

                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenSettingACustomFactoryThenItMustBeUsed()
        {
            ILoggerFactory builderFactory = null;
            var originalFactory = new LoggerFactory();

            using (var builder = new MigratorHostBuilder())
            {
                builder.SetLoggerFactory(originalFactory);
                builder.AddLoggingConfigurator((f, cfg) =>
                {
                    builderFactory = f;
                });
                builder.Build<IMigrationContext>();

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
                builder.AddLoggingConfigurator((f, cfg) =>
                {
                    builderFactory = f;
                });
                builder.Build<IMigrationContext>();

                Assert.NotNull(builderFactory);
                Assert.Same(originalFactory, builderFactory);
            }
        }

        [Fact]
        public void GivenAHostBuilderWhenUsingNullFactoryThenArgumentNullExceptionMustBeThrown()
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
        public void GivenAHostBuilderWhenAddingLoggingConfiguratorsThenAllHandlersMustBeRun()
        {
            var runCount = 0;

            using (var builder = new MigratorHostBuilder())
            {
                builder.AddLoggingConfigurator((f, cfg) =>
                {
                    ++runCount;
                });
                builder.AddLoggingConfigurator((f, cfg) =>
                {
                    ++runCount;
                });
                builder.Build<IMigrationContext>();

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
                builder.Build<IMigrationContext>();

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
                builder.AddServiceConfigurator((s, f, cfg) =>
                {
                    services = s;
                    factory = f;
                });
                builder.Build<IMigrationContext>();

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
                builder.AddServiceConfigurator((s, f, cfg) =>
                {
                    ++runCount;
                });
                builder.AddServiceConfigurator((s, f, cfg) =>
                {
                    ++runCount;
                });
                builder.Build<IMigrationContext>();

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
                .ConfigureServices((s,f) =>
                {
                    ++runCount;
                })
                .ConfigureServices(s =>
                {
                    ++runCount;
                }))
            {
                builder.Build<IMigrationContext>();

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
                builder.SetServiceProviderBuilder((s, f, cfg) =>
                {
                    services = s;
                    factory = f;

                    return s.BuildServiceProvider();
                });
                builder.Build<IMigrationContext>();

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
                .UseServiceProvider((s, f) =>
                {
                    services = s;
                    factory = f;

                    return s.BuildServiceProvider();
                }))
            {
                builder.Build<IMigrationContext>();

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
    }
}
