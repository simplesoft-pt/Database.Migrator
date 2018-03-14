using System;
using System.Threading;

namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer
{
    public class ExistingDatabaseFixture : SqlServerDatabaseFixture
    {
        public ExistingDatabaseFixture() : base(
            "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "Data Source=.; Initial Catalog=MigratorTest; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "MigratorTest")
        {
            PrepareDatabase();

            Context = new SqlServerMigratorTestContext(Options);
            Manager = new SqlServerMigrationManager<SqlServerMigratorTestContext>(
                Context, LoggingManager.LoggerFactory);
        }

        public SqlServerMigratorTestContext Context { get; private set; }

        public SqlServerMigrationManager<SqlServerMigratorTestContext> Manager { get; private set; }

        #region Overrides of SqlServerDatabaseFixture

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
            using (var context = new SqlServerMigratorTestContext(Options))
            {
                var manager = new SqlServerMigrationManager<SqlServerMigratorTestContext>(context, LoggingManager.LoggerFactory);

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