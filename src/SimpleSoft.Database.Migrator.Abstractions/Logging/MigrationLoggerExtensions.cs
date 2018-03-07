using System;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extensions for the <see cref="IMigrationLogger"/> interface.
    /// </summary>
    public static class MigrationLoggerExtensions
    {
        private static readonly string[] EmptyArgs = new string[0];

        /// <summary>
        /// Creates a logger scope with the given information.
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="messageFormat">The message format</param>
        /// <returns>The disposable logger scope</returns>
        public static IDisposable Scope(this IMigrationLogger logger, string messageFormat) =>
            logger.Scope(messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="level">The log level</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void Log(this IMigrationLogger logger, MigrationLogLevel level, Exception exception, string messageFormat) => 
            logger.Log(level, exception, messageFormat);

        /// <summary>
        /// Logs the given log entry with trace level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void LogTrace(this IMigrationLogger logger, Exception exception, string messageFormat) =>
            logger.Log(MigrationLogLevel.Trace, exception, messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry with trace level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        public static void LogTrace(this IMigrationLogger logger, Exception exception, string messageFormat, params string[] args) =>
            logger.Log(MigrationLogLevel.Trace, exception, messageFormat, args);

        /// <summary>
        /// Logs the given log entry with debug level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void LogDebug(this IMigrationLogger logger, Exception exception, string messageFormat) =>
            logger.Log(MigrationLogLevel.Debug, exception, messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry with debug level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        public static void LogDebug(this IMigrationLogger logger, Exception exception, string messageFormat, params string[] args) =>
            logger.Log(MigrationLogLevel.Debug, exception, messageFormat, args);

        /// <summary>
        /// Logs the given log entry with information level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void LogInformation(this IMigrationLogger logger, Exception exception, string messageFormat) =>
            logger.Log(MigrationLogLevel.Information, exception, messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry with information level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        public static void LogInformation(this IMigrationLogger logger, Exception exception, string messageFormat, params string[] args) =>
            logger.Log(MigrationLogLevel.Information, exception, messageFormat, args);

        /// <summary>
        /// Logs the given log entry with warning level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void LogWarning(this IMigrationLogger logger, Exception exception, string messageFormat) =>
            logger.Log(MigrationLogLevel.Warning, exception, messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry with warning level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        public static void LogWarning(this IMigrationLogger logger, Exception exception, string messageFormat, params string[] args) =>
            logger.Log(MigrationLogLevel.Warning, exception, messageFormat, args);

        /// <summary>
        /// Logs the given log entry with error level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void LogError(this IMigrationLogger logger, Exception exception, string messageFormat) =>
            logger.Log(MigrationLogLevel.Error, exception, messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry with error level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        public static void LogError(this IMigrationLogger logger, Exception exception, string messageFormat, params string[] args) =>
            logger.Log(MigrationLogLevel.Error, exception, messageFormat, args);

        /// <summary>
        /// Logs the given log entry with fatal level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        public static void LogFatal(this IMigrationLogger logger, Exception exception, string messageFormat) =>
            logger.Log(MigrationLogLevel.Fatal, exception, messageFormat, EmptyArgs);

        /// <summary>
        /// Logs the given log entry with fatal level
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        public static void LogFatal(this IMigrationLogger logger, Exception exception, string messageFormat, params string[] args) =>
            logger.Log(MigrationLogLevel.Fatal, exception, messageFormat, args);
    }
}