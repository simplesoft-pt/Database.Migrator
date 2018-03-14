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
    /// Manages migration states
    /// </summary>
    public interface IMigrationManager<out TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// Prepares the database for migrations support.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task PrepareDatabaseAsync(CancellationToken ct);

        /// <summary>
        /// Adds the given migration information to the database.
        /// </summary>
        /// <param name="migrationId">The migration identifier</param>
        /// <param name="className">The migration class name</param>
        /// <param name="description">The migration description</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task AddMigrationAsync(string migrationId, string className, string description, CancellationToken ct);

        /// <summary>
        /// Returns a collection of all migrations currently applied
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<IReadOnlyCollection<string>> GetAllMigrationsAsync(CancellationToken ct);

        /// <summary>
        /// Gets the most recent migration identifier from the database 
        /// or <code>null</code> if none is found.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<string> GetMostRecentMigrationIdAsync(CancellationToken ct);

        /// <summary>
        /// Removes the most recent migration from the database indicating success if
        /// any migration was found.
        /// </summary>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<bool> RemoveMostRecentMigrationAsync(CancellationToken ct);
    }
}