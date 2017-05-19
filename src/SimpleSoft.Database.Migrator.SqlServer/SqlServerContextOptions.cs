using System;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// SQL Server context options
    /// </summary>
    public class SqlServerContextOptions
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SqlServerContextOptions(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be whitespace.", nameof(connectionString));

            ConnectionString = connectionString;
        }

        /// <summary>
        /// The connection string to be used
        /// </summary>
        public string ConnectionString { get; }
    }

    /// <summary>
    /// SQL Server context options
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class SqlServerContextOptions<TContext> : SqlServerContextOptions
        where TContext : IRelationalMigrationContext
    {
        /// <inheritdoc />
        public SqlServerContextOptions(string connectionString) : base(connectionString)
        {

        }
    }
}