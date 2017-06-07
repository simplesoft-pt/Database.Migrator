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
    /// Represents a migration
    /// </summary>
    /// <typeparam name="TContext">The migration context</typeparam>
    public abstract class Migration<TContext> : IMigration<TContext> 
        where TContext : IMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Migration(TContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Context = context;
        }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public TContext Context { get; }

        /// <inheritdoc />
        public bool RunInTransaction { get; set; } = true;

        /// <inheritdoc />
        public virtual Task ApplyAsync(CancellationToken ct)
        {
            throw new NotImplementedException(
                "This migration apply method was not overriden. This is intended to prevent the migration history to change without the developer beeing explicit by overriden this method.");
        }
    }
}