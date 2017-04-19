using System;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class SqlServerMigrationManagerTests : IClassFixture<ExistingSqlDatabaseFixture>
    {
        private readonly ExistingSqlDatabaseFixture _fixture;

        public SqlServerMigrationManagerTests(ExistingSqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GivenANewManagerWhenPassingNullParametersThenArgumentNullExceptionMustBeThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(
                    null, LoggingManager.CreateTestLogger<SqlServerMigrationManager<MigratorTestContext>>());
                Assert.NotNull(manager);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(_fixture.Context, null);
                Assert.NotNull(manager);
            });
        }
    }
}
