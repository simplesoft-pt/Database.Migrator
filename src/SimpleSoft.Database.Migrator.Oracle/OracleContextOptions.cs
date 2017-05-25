using System;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Oracle context options
    /// </summary>
    public class OracleContextOptions
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public OracleContextOptions(string connectionString)
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
    /// Oracle context options
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class OracleContextOptions<TContext> : OracleContextOptions
        where TContext : IRelationalMigrationContext
    {
        /// <inheritdoc />
        public OracleContextOptions(string connectionString) : base(connectionString)
        {

        }
    }
}
