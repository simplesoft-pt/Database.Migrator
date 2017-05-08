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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Represents a configured migrator host
    /// </summary>
    public interface IMigratorHost<TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// The migration metadata information, sorted ascending by <see cref="MigrationMetadata{TContext}.Id"/>.
        /// </summary>
        IEnumerable<MigrationMetadata<TContext>> MigrationMetadatas { get; }

        /// <summary>
        /// Applies all the missing migrations to the database, up to the most recent one.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task ApplyMigrationsAsync(CancellationToken ct);

        /// <summary>
        /// Applies all the missing migrations to the database, up to the given one.
        /// </summary>
        /// <param name="migrationId">The migration id</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task ApplyMigrationsStoppingAtAsync(string migrationId, CancellationToken ct);
    }
}