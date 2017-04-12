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
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Relational
{
    /// <summary>
    /// The relational migration context
    /// </summary>
    public abstract class RelationalMigrationContext : MigrationContext, IRelationalMigrationContext, IDisposable
    {
        private static readonly Task CompletedTask = Task.FromResult(true);
        
        private bool _disposed;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RelationalMigrationContext(IDbConnection connection, ILogger<RelationalMigrationContext> logger)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            Connection = connection;
            Logger = logger;
        }

        /// <inheritdoc />
        ~RelationalMigrationContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// The logger used by this instance
        /// </summary>
        protected ILogger<RelationalMigrationContext> Logger { get; }

        /// <inheritdoc />
        public IDbConnection Connection { get; private set; }

        /// <inheritdoc />
        public IDbTransaction Transaction { get; private set; }

        /// <inheritdoc />
        public IsolationLevel DefaultIsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Transaction.Dispose();
                Connection.Dispose();
            }

            Transaction = null;
            Connection = null;

            _disposed = true;
        }

        #endregion

        #region Overrides of MigrationContext

        /// <inheritdoc />
        public override async Task PrepareAsync(CancellationToken ct)
        {
            FailIfDisposed();

            var dbConnection = Connection as DbConnection;
            if (dbConnection == null)
                Connection.Open();
            else
                await dbConnection.OpenAsync(ct).ConfigureAwait(false);

            Transaction = Connection.BeginTransaction(DefaultIsolationLevel);
        }

        /// <inheritdoc />
        public override Task PersistAsync(CancellationToken ct)
        {
            FailIfDisposed();

            Transaction.Commit();

            return CompletedTask;
        }

        /// <inheritdoc />
        public override Task RollbackAsync(CancellationToken ct)
        {
            FailIfDisposed();

            Transaction.Rollback();

            return CompletedTask;
        }

        #endregion

        #region QuerySingleAsync

        /// <inheritdoc />
        public async Task<T> QuerySingleAsync<T>(
            string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            int timeout;
            AssertCommandParameters(
                transaction, commandTimeout, out transaction, out timeout);

            LogQuery(sql, transaction, timeout);

            return await Connection.QuerySingleAsync<T>(sql, param, transaction, timeout, commandType);
        }

        #endregion

        private void FailIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RelationalMigrationContext));
        }

        /// <summary>
        /// Logs the given query information
        /// </summary>
        /// <param name="query">The query to log</param>
        /// <param name="transaction">The transaction currently used</param>
        /// <param name="commandTimeout">The command timeout</param>
        protected void LogQuery(string query, IDbTransaction transaction, int commandTimeout)
        {
            Logger.LogDebug(@"
Executing sql statement in database.
    Is in transaction? {isInTransaction}
    Command Timeout: {commandTimeout}

SQL to execute:
{sqlStatement}",
                transaction != null, commandTimeout, query);
        }

        /// <summary>
        /// Asserts the given parameters to the ones to be used by the command.
        /// </summary>
        /// <param name="transaction">The transaction to assert</param>
        /// <param name="commandTimeout">The command timeout to assert</param>
        /// <param name="tx">The given transaction of, if null, the currently set to <see cref="Transaction"/></param>
        /// <param name="timeout">The given command timeout or, if null, the timeout from <see cref="Connection"/></param>
        protected void AssertCommandParameters(
            IDbTransaction transaction, int? commandTimeout, out IDbTransaction tx, out int timeout)
        {
            tx = transaction ?? Transaction;
            timeout = commandTimeout ?? Connection.ConnectionTimeout;
        }
    }

    /// <summary>
    /// The relational migration context
    /// </summary>
    /// <typeparam name="TOptions">The migration options</typeparam>
    public class RelationalMigrationContext<TOptions> : RelationalMigrationContext, IRelationalMigrationContext<TOptions>
        where TOptions : MigrationOptions
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="options">The context options</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelationalMigrationContext(TOptions options, IDbConnection connection, ILogger<RelationalMigrationContext<TOptions>> logger) 
            : base(connection, logger)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options;
        }

        #region Implementation of IMigrationContext<out TOptions>

        /// <inheritdoc />
        public TOptions Options { get; }

        #endregion
    }
}