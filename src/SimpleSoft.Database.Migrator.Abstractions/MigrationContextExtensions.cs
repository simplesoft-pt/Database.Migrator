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
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extension methods for <see cref="IMigrationContext"/>
    /// </summary>
    public static class MigrationContextExtensions
    {
        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="openTransaction">Should a transaction be open?</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync(
            this IMigrationContext context, Func<Task> action,
            bool openTransaction, CancellationToken ct = default(CancellationToken))
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.PrepareAsync(openTransaction, ct).ConfigureAwait(false);
            try
            {
                await action().ConfigureAwait(false);

                await context.PersistAsync(ct).ConfigureAwait(false);
            }
            catch (Exception)
            {
                await context.RollbackAsync(ct).ConfigureAwait(false);
                throw;
            }
        }

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="openTransaction">Should a transaction be open?</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TResult>(
            this IMigrationContext context, Func<Task<TResult>> action,
            bool openTransaction, CancellationToken ct = default(CancellationToken))
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.PrepareAsync(openTransaction, ct).ConfigureAwait(false);
            try
            {
                var result = await action().ConfigureAwait(false);

                await context.PersistAsync(ct).ConfigureAwait(false);

                return result;
            }
            catch (Exception)
            {
                await context.RollbackAsync(ct).ConfigureAwait(false);
                throw;
            }
        }
    }
}