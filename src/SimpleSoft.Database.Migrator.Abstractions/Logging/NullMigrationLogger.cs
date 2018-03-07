using System;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Null migrations logger
    /// </summary>
    public class NullMigrationLogger : IMigrationLogger
    {
        /// <summary>
        /// Singleton instance for null loggers
        /// </summary>
        public static readonly NullMigrationLogger Default = new NullMigrationLogger();

        /// <inheritdoc />
        public void Log(MigrationLogLevel level, Exception exception, string messageFormat, params string[] args)
        {

        }

        /// <inheritdoc />
        public bool IsEnabled(MigrationLogLevel level) => false;

        /// <inheritdoc />
        public IDisposable Scope(string messageFormat, params string[] args) => null;
    }
}