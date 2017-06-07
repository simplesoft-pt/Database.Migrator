using System;
using System.Threading;
using Oracle.ManagedDataAccess.Client;

namespace SimpleSoft.Database.Migrator.Tests.Relational.Oracle
{
    public class ExistingDatabaseFixture : OracleDatabaseFixture
    {
        public ExistingDatabaseFixture() : base(
            "DATA SOURCE=LocalOracle/xe;USER ID=MIGRATORMASTER;PASSWORD=MIGRATORMASTER;VALIDATE CONNECTION=TRUE;",
            "DATA SOURCE=LocalOracle/xe;USER ID=MIGRATORTEST;PASSWORD=MIGRATORTEST;VALIDATE CONNECTION=TRUE;",
            "MIGRATORTEST", "MIGRATORTEST")
        {
            PrepareDatabase();

            Context = new MigratorTestContext(Connection);
            Manager = new OracleMigrationManager<MigratorTestContext>(
                Context, new DefaultNamingNormalizer<MigratorTestContext>(),
                LoggingManager.CreateTestLogger<OracleMigrationManager<MigratorTestContext>>());
        }

        public MigratorTestContext Context { get; private set; }

        public OracleMigrationManager<MigratorTestContext> Manager { get; private set; }

        #region Overrides of OracleDatabaseFixture

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Context?.Dispose();
            }

            Manager = null;
            Context = null;
        }

        #endregion

        private void PrepareDatabase()
        {
            var ct = CancellationToken.None;
            using (var connection = new OracleConnection(ConnectionString))
            using (var context = new MigratorTestContext(connection))
            {
                var manager = new OracleMigrationManager<MigratorTestContext>(
                    context, new DefaultNamingNormalizer<MigratorTestContext>(),
                    LoggingManager.CreateTestLogger<OracleMigrationManager<MigratorTestContext>>());

                manager.PrepareDatabaseAsync(ct)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                string migrationId;
                string className;
                MigrationsTestHelper.GenerateMigrationInfo(
                    DateTimeOffset.UtcNow, out migrationId, out className);

                //  Existing static migration
                manager.AddMigrationAsync(migrationId, className, null, ct)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
