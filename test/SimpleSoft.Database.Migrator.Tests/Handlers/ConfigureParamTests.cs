using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Handlers
{
    public class ConfigureParamTests
    {
        [Fact]
        public void GivenANewInstanceThenAllPropertiesReferenceMustBeSameAsParams()
        {
            var serviceCollection = new ServiceCollection().BuildServiceProvider();
            var factory = new LoggerFactory();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var param = new ConfigureParam(serviceCollection, factory, configuration);

            Assert.NotNull(param.ServiceProvider);
            Assert.Same(serviceCollection, param.ServiceProvider);

            Assert.NotNull(param.Factory);
            Assert.Same(factory, param.Factory);

            Assert.NotNull(param.Configuration);
            Assert.Same(configuration, param.Configuration);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            ConfigureParam param = null;
            var serviceCollection = new ServiceCollection().BuildServiceProvider();
            var factory = new LoggerFactory();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigureParam(null, factory, configuration);
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigureParam(serviceCollection, null, configuration);
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigureParam(serviceCollection, factory, null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}