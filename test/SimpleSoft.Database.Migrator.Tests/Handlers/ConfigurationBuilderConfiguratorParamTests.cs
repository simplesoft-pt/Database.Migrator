using System;
using Microsoft.Extensions.Configuration;
using SimpleSoft.Database.Migrator.Hosting;
using SimpleSoft.Database.Migrator.Hosting.Handlers;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Handlers
{
    public class ConfigurationBuilderConfiguratorParamTests
    {
        [Fact]
        public void GivenANewInstanceThenAllPropertiesReferenceMustBeSameAsParams()
        {
            var environment = new HostingEnvironment();
            var builder = new ConfigurationBuilder();
            var param = new ConfigurationBuilderConfiguratorParam(builder, environment);

            Assert.NotNull(param.Builder);
            Assert.Same(builder, param.Builder);

            Assert.NotNull(param.Environment);
            Assert.Same(environment, param.Environment);
        }

        [Fact]
        public void GivenANewInstanceWithNullParamsThenArgumentNullExceptionMustBeThrown()
        {
            ConfigurationBuilderConfiguratorParam param = null;
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigurationBuilderConfiguratorParam(null, new HostingEnvironment());
            });

            Assert.NotNull(ex);
            Assert.Null(param);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                param = new ConfigurationBuilderConfiguratorParam(new ConfigurationBuilder(), null);
            });

            Assert.NotNull(ex);
            Assert.Null(param);
        }
    }
}