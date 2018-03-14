using System;
using System.Threading;

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

            Context = new OracleMigratorTestContext(Options);
            Manager = new OracleMigrationManager<OracleMigratorTestContext>(
                Context, LoggingManager.LoggerFactory);
        }

        public OracleMigratorTestContext Context { get; private set; }

        public OracleMigrationManager<OracleMigratorTestContext> Manager { get; private set; }

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
            using (var context = new OracleMigratorTestContext(Options))
            {
                var manager = new OracleMigrationManager<OracleMigratorTestContext>(context, LoggingManager.LoggerFactory);

                manager.PrepareDatabaseAsync(ct)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                MigrationsTestHelper.GenerateMigrationInfo(
                    DateTimeOffset.UtcNow, out var migrationId, out var className);

                //  Existing static migration
                manager.AddMigrationAsync(migrationId, className, null, ct)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
