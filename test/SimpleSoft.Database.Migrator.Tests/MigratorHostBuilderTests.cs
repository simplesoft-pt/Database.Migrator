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
                builder.AddLoggingConfigurator(f =>
                {
                    factory = f;
                });
                builder.Build();

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
                builder.AddLoggingConfigurator(f =>
                {
                    builderFactory = f;
                });
                builder.Build();

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
                builder.AddLoggingConfigurator(f =>
                {
                    builderFactory = f;
                });
                builder.Build();

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
                builder.AddLoggingConfigurator(f =>
                {
                    ++runCount;
                });
                builder.AddLoggingConfigurator(f =>
                {
                    ++runCount;
                });
                builder.Build();

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
                builder.Build();

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
                builder.AddServiceConfigurator((s, f) =>
                {
                    services = s;
                    factory = f;
                });
                builder.Build();

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
                builder.AddServiceConfigurator((s, f) =>
                {
                    ++runCount;
                });
                builder.AddServiceConfigurator((s, f) =>
                {
                    ++runCount;
                });
                builder.Build();

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
                builder.Build();

                Assert.NotEmpty(builder.ServiceConfigurationHandlers);
                Assert.Equal(2, builder.ServiceConfigurationHandlers.Count);
                Assert.Equal(2, runCount);
            }
        }

        #endregion
    }
}
