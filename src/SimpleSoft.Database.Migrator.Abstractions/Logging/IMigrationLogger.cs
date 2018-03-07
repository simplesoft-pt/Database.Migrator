using System;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Logger used by migrations classes
    /// </summary>
    public interface IMigrationLogger
    {
        /// <summary>
        /// Logs the given log entry
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        void Log(MigrationLogLevel level, Exception exception, string messageFormat, params string[] args);

        /// <summary>
        /// Checks if a given log level is enabled
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabled(MigrationLogLevel level);

        /// <summary>
        /// Creates a logger scope with the given information.
        /// </summary>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        /// <returns>The disposable logger scope</returns>
        IDisposable Scope(string messageFormat, params string[] args);
    }
}
