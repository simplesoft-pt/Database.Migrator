#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 João Simões
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Data;
using System.Data.SqlClient;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The SQL Server migration context
    /// </summary>
    public class SqlServerMigrationContext : RelationalMigrationContext, ISqlServerMigrationContext
    {
        private IDbConnection _connection;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The context options</param>
        /// <param name="normalizer">The naming normalizer</param>
        /// <param name="loggerFactory">An optional class logger factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlServerMigrationContext(ISqlServerMigrationOptions options, INamingNormalizer normalizer, IMigrationLoggerFactory loggerFactory = null) 
            : base(options, normalizer, loggerFactory)
        {
            Options = options;
        }

        /// <inheritdoc />
        public new ISqlServerMigrationOptions Options { get; }

        /// <inheritdoc />
        public override IDbConnection Connection
        {
            get => _connection ?? (_connection = new SqlConnection(Options.ConnectionString));
            protected set => _connection = value;
        }
    }
}