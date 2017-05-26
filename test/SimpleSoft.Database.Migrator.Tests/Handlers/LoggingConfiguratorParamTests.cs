using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleSoft.Database.Migrator.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Handlers
{
    public class LoggingConfiguratorParamTests
    {
        [Fact]
        public void GivenANewInstanceThenAllPropertiesReferenceMustBeSameAsParams()
        {
            var factory = new LoggerFactory();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var environment = new HostingEnvironment();

            var param = new LoggingConfiguratorParam(factory, configuration, environment);

            Assert.NotNull(param.Factory);
            Assert.Same(factory, param.Factory);

            Assert.NotNull(param.Configuration);
            Assert.Same(configuration, param.Configuration);

            Assert.NotNull(param.Environment);
            Assert.Same(environment, param.Environment);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            LoggingConfiguratorParam param = null;
            var factory = new LoggerFactory();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var environment = new HostingEnvironment();

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new LoggingConfiguratorParam(null, configuration, environment);
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new LoggingConfiguratorParam(factory, null, environment);
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new LoggingConfiguratorParam(factory, configuration, null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}