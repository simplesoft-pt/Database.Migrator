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
            var param = new ConfigurationConfiguratorParam(configuration);

            Assert.NotNull(param.Configuration);
            Assert.Same(configuration, param.Configuration);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            ConfigurationConfiguratorParam param = null;
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigurationConfiguratorParam(null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}