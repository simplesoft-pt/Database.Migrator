namespace SimpleSoft.Database.Migrator.DependencyInjection
{
    /// <summary>
    /// Factory used to build <see cref="IMigrationInstanceScope"/> instances.
    /// </summary>
    public interface IMigrationInstanceScopeFactory
    {
        /// <summary>
        /// Builds a new instance scope
        /// </summary>
        /// <returns>The instance scope</returns>
        IMigrationInstanceScope Build();
    }
}