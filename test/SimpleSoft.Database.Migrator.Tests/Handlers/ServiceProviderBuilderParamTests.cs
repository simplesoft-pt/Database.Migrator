using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Handlers
{
    public class ServiceProviderBuilderParamTests
    {
        [Fact]
        public void GivenANewInstanceThenAllPropertiesReferenceMustBeSameAsParams()
        {
            var serviceCollection = new ServiceCollection();
            var factory = new LoggerFactory();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var param = new ServiceProviderBuilderParam(serviceCollection, factory, configuration);

            Assert.NotNull(param.ServiceCollection);
            Assert.Same(serviceCollection, param.ServiceCollection);

            Assert.NotNull(param.Factory);
            Assert.Same(factory, param.Factory);

            Assert.NotNull(param.Configuration);
            Assert.Same(configuration, param.Configuration);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            ServiceProviderBuilderParam param = null;
            var serviceCollection = new ServiceCollection();
            var factory = new LoggerFactory();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ServiceProviderBuilderParam(null, factory, configuration);
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ServiceProviderBuilderParam(serviceCollection, null, configuration);
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ServiceProviderBuilderParam(serviceCollection, factory, null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}