namespace SimpleSoft.Database.Migrator.Tests.SqlServer
{
    public class EmptySqlDatabaseFixture : SqlServerDatabaseFixture
    {
        public EmptySqlDatabaseFixture() : base(
            "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "Data Source=.; Initial Catalog=MigratorTestEmpty; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "MigratorTestEmpty", true)
        {
            Context = new MigratorTestContext(Connection);
        }

        public MigratorTestContext Context { get; private set; }

        #region Overrides of SqlServerDatabaseFixture

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Context?.Dispose();
            }
            Context = null;
        }

        #endregion
    }
}