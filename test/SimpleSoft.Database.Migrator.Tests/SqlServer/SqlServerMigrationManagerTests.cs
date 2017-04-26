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
        public async Task GivenANewMigrationWhenPassingNullParametersThenArgumentNullExceptionMustBeThrown()
        {
            var ct = CancellationToken.None;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync(null, "...", ct);
            });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("...", null, ct);
            });
        }

        [Fact]
        public async Task GivenANewMigrationWhenPassingEmptyParametersThenArgumentExceptionMustBeThrown()
        {
            var ct = CancellationToken.None;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync(string.Empty, "...", ct);
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("...", string.Empty, ct);
            });
        }

        [Fact]
        public async Task GivenANewMigrationWhenPassingWhiteSpaceParametersThenArgumentExceptionMustBeThrown()
        {
            var ct = CancellationToken.None;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync(
                    "    ", "...", ct);
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("...", "    ", ct);
            });
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenAddingANewOneNoExceptionIsThrown()
        {
            var migrationId = string.Concat(
                "Migration", DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fffffff"));

            //  Existing static migration
            await _fixture.Manager.AddMigrationAsync(
                migrationId, string.Concat("SimpleSoft.Database.Migrator.Tests.SqlServer.", migrationId),
                CancellationToken.None);
        }
    }
}
