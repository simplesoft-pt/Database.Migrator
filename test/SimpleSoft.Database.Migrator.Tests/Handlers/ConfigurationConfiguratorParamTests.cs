using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SimpleSoft.Database.Migrator.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Handlers
{
    public class ConfigurationConfiguratorParamTests
    {
        [Fact]
        public void GivenANewInstanceThenAllPropertiesReferenceMustBeSameAsParams()
        {
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var environment = new HostingEnvironment();
            var param = new ConfigurationConfiguratorParam(configuration, environment);

            Assert.NotNull(param.Configuration);
            Assert.Same(configuration, param.Configuration);

            Assert.NotNull(param.Environment);
            Assert.Same(environment, param.Environment);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            ConfigurationConfiguratorParam param = null;
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigurationConfiguratorParam(null, new HostingEnvironment());
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigurationConfiguratorParam(
                    new ConfigurationRoot(new List<IConfigurationProvider>()), null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}