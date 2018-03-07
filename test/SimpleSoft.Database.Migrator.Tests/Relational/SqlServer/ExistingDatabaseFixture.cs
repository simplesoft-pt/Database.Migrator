using System;
using System.Data.SqlClient;
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

            Context = new MigratorTestContext(Connection);
            Manager = new SqlServerMigrationManager<MigratorTestContext>(
                Context, new DefaultNamingNormalizer<MigratorTestContext>(), LoggingManager.LoggerFactory);
        }

        public MigratorTestContext Context { get; private set; }

        public SqlServerMigrationManager<MigratorTestContext> Manager { get; private set; }

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
            using (var connection = new SqlConnection(ConnectionString))
            using (var context = new MigratorTestContext(connection))
            {
                var manager = new SqlServerMigrationManager<MigratorTestContext>(
                    context, new DefaultNamingNormalizer<MigratorTestContext>(), LoggingManager.LoggerFactory);

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