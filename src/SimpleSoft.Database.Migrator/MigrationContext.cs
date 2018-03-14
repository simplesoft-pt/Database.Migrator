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
    /// The migration context
    /// </summary>
    public abstract class MigrationContext : IMigrationContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The context options</param>
        /// <param name="normalizer">The naming normalizer</param>
        /// <param name="loggerFactory">An optional class logger factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MigrationContext(IMigrationOptions options, INamingNormalizer normalizer, IMigrationLoggerFactory loggerFactory = null)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Normalizer = normalizer ?? throw new ArgumentNullException(nameof(normalizer));
            Logger = loggerFactory?.Get(GetType().FullName) ?? NullMigrationLogger.Default;

            NormalizedName = normalizer.Normalize(options.ContextName);
        }

        /// <summary>
        /// The logger used by this instance
        /// </summary>
        protected IMigrationLogger Logger { get; }

        /// <inheritdoc />
        public IMigrationOptions Options { get; }

        /// <inheritdoc />
        public INamingNormalizer Normalizer { get; }

        /// <inheritdoc />
        public string NormalizedName { get; }

        /// <inheritdoc />
        public virtual Task PrepareAsync(bool openTransaction, CancellationToken ct)
        {
#if NET451
            return TaskEx.CompletedTask;
#else
            return Task.CompletedTask;
#endif
        }

        /// <inheritdoc />
        public virtual Task PersistAsync(CancellationToken ct)
        {
#if NET451
            return TaskEx.CompletedTask;
#else
            return Task.CompletedTask;
#endif
        }

        /// <inheritdoc />
        public virtual Task RollbackAsync(CancellationToken ct)
        {
#if NET451
            return TaskEx.CompletedTask;
#else
            return Task.CompletedTask;
#endif
        }
    }
}
