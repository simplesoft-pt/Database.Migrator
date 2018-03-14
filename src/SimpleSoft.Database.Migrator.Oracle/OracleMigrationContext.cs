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
using Oracle.ManagedDataAccess.Client;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The Oracle migration context
    /// </summary>
    public class OracleMigrationContext : RelationalMigrationContext, IOracleMigrationContext
    {
        private IDbConnection _connection;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The context options</param>
        /// <param name="normalizer">The naming normalizer</param>
        /// <param name="loggerFactory">An optional class logger factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OracleMigrationContext(IOracleMigrationOptions options, INamingNormalizer normalizer, IMigrationLoggerFactory loggerFactory = null) 
            : base(options, normalizer, loggerFactory)
        {
            Options = options;
        }

        /// <inheritdoc />
        public new IOracleMigrationOptions Options { get; }

        /// <inheritdoc />
        public override IDbConnection Connection
        {
            get => _connection ?? (_connection = new OracleConnection(Options.ConnectionString));
            protected set => _connection = value;
        }
    }
}
