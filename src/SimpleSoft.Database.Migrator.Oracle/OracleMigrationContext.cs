using System;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace SimpleSoft.Database.Migrator.Oracle
{
    /// <summary>
    /// The Oracle migration context
    /// </summary>
    public class OracleMigrationContext<TOptions> : RelationalMigrationContext, IOracleMigrationContext<TOptions> 
        where TOptions : OracleContextOptions
    {
        /// <inheritdoc />
        public OracleMigrationContext(TOptions options, ILogger<OracleMigrationContext<TOptions>> logger) 
            : base(new OracleConnection(options.ConnectionString), logger)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options;
        }

        #region Implementation of IOracleMigrationContext<out TOptions>

        /// <inheritdoc />
        public TOptions Options { get; }

        #endregion
    }
}
