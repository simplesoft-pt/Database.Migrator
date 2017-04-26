using System;
using System.Threading;
using System.Threading.Tasks;
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

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenGettingTheMostRecentThenOneBustBeReturned()
        {
            var migrationId = await _fixture.Manager.GetMostRecentMigrationIdAsync(CancellationToken.None);

            Assert.NotNull(migrationId);
            Assert.NotEmpty(migrationId);
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenAddingANewOneNoExceptionIsThrown()
        {
            var ct = CancellationToken.None;

            var mostRecentMigrationId = await _fixture.Manager.GetMostRecentMigrationIdAsync(ct);

            var testMigrationId = string.Concat(
                "V", (int.Parse(mostRecentMigrationId.Replace("V", string.Empty)) + 1).ToString("D5"));

            await _fixture.Manager.AddMigrationAsync(
                testMigrationId, $"SimpleSoft.Database.Migrator.Tests.SqlServer.{testMigrationId}", ct);
        }
    }
}
