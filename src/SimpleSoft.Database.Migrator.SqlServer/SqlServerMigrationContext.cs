using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The SQL Server migration context
    /// </summary>
    public class SqlServerMigrationContext<TOptions> : RelationalMigrationContext, ISqlServerMigrationContext<TOptions>
        where TOptions : SqlServerContextOptions
    {
        /// <inheritdoc />
        public SqlServerMigrationContext(TOptions options, ILogger<SqlServerMigrationContext<TOptions>> logger) 
            : base(new SqlConnection(options.ConnectionString), logger)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options;
        }

        #region Implementation of ISqlServerMigrationContext<out TOptions>

        /// <inheritdoc />
        public TOptions Options { get; }

        #endregion
    }
}