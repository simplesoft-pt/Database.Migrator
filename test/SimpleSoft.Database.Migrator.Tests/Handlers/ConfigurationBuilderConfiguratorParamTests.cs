using System;
using Microsoft.Extensions.Configuration;
using SimpleSoft.Database.Migrator.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Handlers
{
    public class ConfigurationBuilderConfiguratorParamTests
    {
        [Fact]
        public void GivenANewInstanceThenAllPropertiesReferenceMustBeSameAsParams()
        {
            var builder = new ConfigurationBuilder();
            var param = new ConfigurationBuilderConfiguratorParam(builder);

            Assert.NotNull(param.Builder);
            Assert.Same(builder, param.Builder);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            ConfigurationBuilderConfiguratorParam param = null;
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigurationBuilderConfiguratorParam(null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}