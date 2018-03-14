namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer
{
    public class EmptyDatabaseFixture : SqlServerDatabaseFixture
    {
        public EmptyDatabaseFixture() : base(
            "Data Source=.; Initial Catalog=master; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "Data Source=.; Initial Catalog=MigratorTestEmpty; Integrated Security=True; Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True;App=SimpleSoft.Database.Migrator.Tests.SqlServer",
            "MigratorTestEmpty", true)
        {
            Context = new SqlServerMigratorTestContext(Options);
        }

        public SqlServerMigratorTestContext Context { get; private set; }

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