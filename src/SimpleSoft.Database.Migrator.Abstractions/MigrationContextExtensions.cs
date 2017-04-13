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
        #region RunAsync

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync(
            this IMigrationContext context, Func<IMigrationContext, CancellationToken, Task> action, CancellationToken ct = default(CancellationToken))
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.PrepareAsync(ct).ConfigureAwait(false);
            try
            {
                await action(context, ct).ConfigureAwait(false);

                await context.PrepareAsync(ct).ConfigureAwait(false);
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
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync(
            this IMigrationContext context, Func<IMigrationContext, Task> action, CancellationToken ct = default(CancellationToken))
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.RunAsync(async (ctx, c) => await action(context).ConfigureAwait(false), ct);
        }

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync(
            this IMigrationContext context, Func<Task> action, CancellationToken ct = default(CancellationToken))
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.RunAsync(async (ctx, c) => await action().ConfigureAwait(false), ct);
        }

        #endregion

        #region RunAsync<TResult>

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TResult>(
            this IMigrationContext context, Func<IMigrationContext, CancellationToken, Task<TResult>> action, CancellationToken ct = default(CancellationToken))
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.PrepareAsync(ct).ConfigureAwait(false);
            try
            {
                var result = await action(context, ct).ConfigureAwait(false);

                await context.PrepareAsync(ct).ConfigureAwait(false);

                return result;
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
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TResult>(
            this IMigrationContext context, Func<IMigrationContext, Task<TResult>> action, CancellationToken ct = default(CancellationToken))
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return await context.RunAsync(async (ctx, c) => await action(context).ConfigureAwait(false), ct);
        }

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TResult>(
            this IMigrationContext context, Func<Task<TResult>> action, CancellationToken ct = default(CancellationToken))
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return await context.RunAsync(async (ctx, c) => await action().ConfigureAwait(false), ct);
        }

        #endregion

        #region RunAsync<TContext>

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync<TContext>(
            this TContext context, Func<TContext, CancellationToken, Task> action, CancellationToken ct = default(CancellationToken))
            where TContext : IMigrationContext
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.PrepareAsync(ct).ConfigureAwait(false);
            try
            {
                await action(context, ct).ConfigureAwait(false);

                await context.PrepareAsync(ct).ConfigureAwait(false);
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
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync<TContext>(
            this TContext context, Func<TContext, Task> action, CancellationToken ct = default(CancellationToken))
            where TContext : IMigrationContext
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.RunAsync(async (ctx, c) =>
            {
                await action(context).ConfigureAwait(false);
            }, ct);
        }

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task RunAsync<TContext>(
            this TContext context, Func<Task> action, CancellationToken ct = default(CancellationToken))
            where TContext : IMigrationContext
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.RunAsync(async (ctx, c) =>
            {
                await action().ConfigureAwait(false);
            }, ct);
        }

        #endregion

        #region RunAsync<TContext, TResult>

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TContext, TResult>(
            this TContext context, Func<TContext, CancellationToken, Task<TResult>> action, CancellationToken ct = default(CancellationToken))
            where TContext : IMigrationContext
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await context.PrepareAsync(ct).ConfigureAwait(false);
            try
            {
                var result = await action(context, ct).ConfigureAwait(false);

                await context.PrepareAsync(ct).ConfigureAwait(false);

                return result;
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
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TContext, TResult>(
            this TContext context, Func<TContext, Task<TResult>> action, CancellationToken ct = default(CancellationToken))
            where TContext : IMigrationContext
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return await context.RunAsync(async (ctx, c) => await action(context).ConfigureAwait(false), ct);
        }

        /// <summary>
        /// Execute the given action inside a context scope. The method
        /// <see cref="IMigrationContext.PrepareAsync"/> will be invoked and,
        /// if no exception is thrown <see cref="IMigrationContext.PersistAsync"/>, otherwise 
        /// <see cref="IMigrationContext.RollbackAsync"/> will be invoked instead.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="action">The action to execute</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        public static async Task<TResult> RunAsync<TContext, TResult>(
            this TContext context, Func<Task<TResult>> action, CancellationToken ct = default(CancellationToken))
            where TContext : IMigrationContext
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return await context.RunAsync(async (ctx, c) => await action().ConfigureAwait(false), ct);
        }

        #endregion
    }
}