namespace SimpleSoft.Database.Migrator.Tests.Relational.Oracle
{
    public class EmptyDatabaseFixture : OracleDatabaseFixture
    {
        public EmptyDatabaseFixture() : base(
            "DATA SOURCE=LocalOracle/xe;USER ID=MIGRATORMASTER;PASSWORD=MIGRATORMASTER;VALIDATE CONNECTION=TRUE;",
            "DATA SOURCE=LocalOracle/xe;USER ID=MIGRATORTESTEMPTY;PASSWORD=MIGRATORTESTEMPTY;VALIDATE CONNECTION=TRUE;",
            "MIGRATORTESTEMPTY", "MIGRATORTESTEMPTY")
        {
            Context = new MigratorTestContext(Connection);
        }

        public MigratorTestContext Context { get; private set; }

        #region Overrides of OracleDatabaseFixture

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
