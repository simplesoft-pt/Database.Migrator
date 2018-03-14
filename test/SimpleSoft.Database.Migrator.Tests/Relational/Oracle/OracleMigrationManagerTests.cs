using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSoft.Database.Migrator.Tests.Relational.Oracle
{
    public class OracleMigrationManagerTests : IClassFixture<ExistingDatabaseFixture>
    {
        private readonly ExistingDatabaseFixture _fixture;

        public OracleMigrationManagerTests(ExistingDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GivenANewManagerWhenPassingNullParametersThenArgumentNullExceptionMustBeThrown()
        {
            OracleMigrationManager<OracleMigratorTestContext> manager;
            Assert.Throws<ArgumentNullException>(() =>
            {
                manager = new OracleMigrationManager<OracleMigratorTestContext>(null, LoggingManager.LoggerFactory);
                Assert.NotNull(manager);
            });

            manager = new OracleMigrationManager<OracleMigratorTestContext>(_fixture.Context);
            Assert.NotNull(manager);
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenGettingTheMostRecentThenOneMustBeReturned()
        {
            var migrationId = await _fixture.Manager.GetMostRecentMigrationIdAsync(CancellationToken.None);

            Assert.NotNull(migrationId);
            Assert.NotEmpty(migrationId);
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenGettingAllThenSomeMustBeReturned()
        {
            var ct = CancellationToken.None;
            MigrationsTestHelper.GenerateMigrationInfo(out string migrationId, out string className);
            await _fixture.Manager.AddMigrationAsync(
                migrationId, className, $"Description for migration {migrationId}", ct);

            var migrationIds = await _fixture.Manager.GetAllMigrationsAsync(ct);

            Assert.NotNull(migrationIds);
            Assert.NotEmpty(migrationIds);
        }

        [Fact]
        public async Task GivenANewMigrationWhenPassingNullParametersThenArgumentNullExceptionMustBeThrown()
        {
            var ct = CancellationToken.None;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync(null, "...", null, ct);
            });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("...", null, null, ct);
            });
        }

        [Fact]
        public async Task GivenANewMigrationWhenPassingEmptyParametersThenArgumentExceptionMustBeThrown()
        {
            var ct = CancellationToken.None;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync(string.Empty, "...", null, ct);
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("...", string.Empty, null, ct);
            });
        }

        [Fact]
        public async Task GivenANewMigrationWhenPassingWhiteSpaceParametersThenArgumentExceptionMustBeThrown()
        {
            var ct = CancellationToken.None;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("    ", "...", null, ct);
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync("...", "    ", null, ct);
            });
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenAddingANewOneThenNoExceptionIsThrown()
        {
            MigrationsTestHelper.GenerateMigrationInfo(out string migrationId, out string className);

            await _fixture.Manager.AddMigrationAsync(
                migrationId, className, $"Description for migration {migrationId}", CancellationToken.None);
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenAddingAnOlderOneThenAnInvalidOperationExceptionMustBeThrown()
        {
            MigrationsTestHelper.GenerateMigrationInfo(out string migrationId, out string className);

            await _fixture.Manager.AddMigrationAsync(
                migrationId, className, $"Description for migration {migrationId}", CancellationToken.None);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                MigrationsTestHelper.GenerateMigrationInfo(
                    DateTimeOffset.UtcNow.AddDays(-1), out migrationId, out className);

                await _fixture.Manager.AddMigrationAsync(
                    migrationId, className, $"Description for migration {migrationId}", CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenAddingAnExistingOneThenAnInvalidOperationExceptionMustBeThrown()
        {
            MigrationsTestHelper.GenerateMigrationInfo(out string migrationId, out string className);

            await _fixture.Manager.AddMigrationAsync(
                migrationId, className, $"Description for migration {migrationId}", CancellationToken.None);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _fixture.Manager.AddMigrationAsync(
                    migrationId, className, $"Description for migration {migrationId}", CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenRemovingTheMostRecentThenTrueMustBeReturned()
        {
            var ct = CancellationToken.None;

            var migrationIdsBeforeRemove = await _fixture.Manager.GetAllMigrationsAsync(ct);

            var removed = await _fixture.Manager.RemoveMostRecentMigrationAsync(ct);

            var migrationIdsAfterRemove = await _fixture.Manager.GetAllMigrationsAsync(ct);

            Assert.True(removed);
            Assert.NotEqual(migrationIdsBeforeRemove.Count, migrationIdsAfterRemove.Count);
            Assert.True(migrationIdsBeforeRemove.Count > migrationIdsAfterRemove.Count);
        }

        [Fact]
        public async Task GivenADatabaseWithMigrationsWhenRemovingAllThenFalseMustEventuallyBeReturned()
        {
            var ct = CancellationToken.None;

            var migrationIds = await _fixture.Manager.GetAllMigrationsAsync(ct);

            //    guard condition to be sure the cicle eventually ends
            var countGuard = migrationIds.Count + 20;

            while (await _fixture.Manager.RemoveMostRecentMigrationAsync(ct))
            {
                --countGuard;
                Assert.NotEqual(0, countGuard);
            }

            migrationIds = await _fixture.Manager.GetAllMigrationsAsync(ct);
            Assert.Empty(migrationIds);
        }
    }
}