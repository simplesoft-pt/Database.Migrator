using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Tests
{
    public static class LoggingManager
    {
        public static readonly IMigrationLoggerFactory LoggerFactory;

        static LoggingManager()
        {
            LoggerFactory = new MigrationLoggerFactory(
                new LoggerFactory()
                    .AddConsole(LogLevel.Trace, true)
                    .AddDebug(LogLevel.Trace));
        }

        private class MigrationLoggerFactory : IMigrationLoggerFactory
        {
            private readonly ILoggerFactory _loggerFactory;

            public MigrationLoggerFactory(ILoggerFactory loggerFactory)
            {
                if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
                _loggerFactory = loggerFactory;
            }

            public IMigrationLogger Get(string name) => new MigrationLogger(_loggerFactory.CreateLogger(name));

            private class MigrationLogger : IMigrationLogger
            {
                private static readonly object[] EmptyArgs = new object[0];
                private readonly ILogger _logger;

                public MigrationLogger(ILogger logger)
                {
                    if (logger == null) throw new ArgumentNullException(nameof(logger));
                    _logger = logger;
                }

                public void Log(MigrationLogLevel level, Exception exception, string messageFormat, params string[] args)
                {
                    switch (level)
                    {
                        case MigrationLogLevel.None:
                            return;
                        case MigrationLogLevel.Trace:
                            if (_logger.IsEnabled(LogLevel.Trace))
                                _logger.LogTrace(0, exception, messageFormat, ToObjectArray(args));
                            return;
                        case MigrationLogLevel.Debug:
                            if (_logger.IsEnabled(LogLevel.Debug))
                                _logger.LogDebug(0, exception, messageFormat, ToObjectArray(args));
                            return;
                        case MigrationLogLevel.Information:
                            if (_logger.IsEnabled(LogLevel.Information))
                                _logger.LogInformation(0, exception, messageFormat, ToObjectArray(args));
                            return;
                        case MigrationLogLevel.Warning:
                            if (_logger.IsEnabled(LogLevel.Warning))
                                _logger.LogWarning(0, exception, messageFormat, ToObjectArray(args));
                            return;
                        case MigrationLogLevel.Error:
                            if (_logger.IsEnabled(LogLevel.Error))
                                _logger.LogError(0, exception, messageFormat, ToObjectArray(args));
                            return;
                        case MigrationLogLevel.Fatal:
                            if (_logger.IsEnabled(LogLevel.Critical))
                                _logger.LogCritical(0, exception, messageFormat, ToObjectArray(args));
                            return;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(level), level, null);
                    }
                }

                public bool IsEnabled(MigrationLogLevel level) =>
                    _logger.IsEnabled(ToLogLevel(level));

                public IDisposable Scope(string messageFormat, params string[] args) =>
                    _logger.BeginScope(messageFormat, ToObjectArray(args));

                private static LogLevel ToLogLevel(MigrationLogLevel level)
                {
                    switch (level)
                    {
                        case MigrationLogLevel.None:
                            return LogLevel.None;
                        case MigrationLogLevel.Trace:
                            return LogLevel.Trace;
                        case MigrationLogLevel.Debug:
                            return LogLevel.Debug;
                        case MigrationLogLevel.Information:
                            return LogLevel.Information;
                        case MigrationLogLevel.Warning:
                            return LogLevel.Warning;
                        case MigrationLogLevel.Error:
                            return LogLevel.Error;
                        case MigrationLogLevel.Fatal:
                            return LogLevel.Critical;
                        default:
                            return LogLevel.None;
                    }
                }

                private static object[] ToObjectArray(string[] args)
                {
                    if (args == null || args.Length == 0)
                        return EmptyArgs;

                    var objArgs = new object[args.Length];
                    for (var i = 0; i < args.Length; i++)
                        objArgs[i] = args[i];

                    return objArgs;
                }
            }
        }
    }
}
