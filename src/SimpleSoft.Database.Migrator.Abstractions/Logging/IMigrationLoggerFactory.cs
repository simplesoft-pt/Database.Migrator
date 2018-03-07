// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Factory for <see cref="IMigrationLogger"/> instances.
    /// </summary>
    public interface IMigrationLoggerFactory
    {
        /// <summary>
        /// Gets a logger with the given name
        /// </summary>
        /// <param name="name">The logger name</param>
        /// <returns>The logger instance</returns>
        IMigrationLogger Get(string name);
    }
}