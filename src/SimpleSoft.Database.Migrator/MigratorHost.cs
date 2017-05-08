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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Represents a configured migrator host
    /// </summary>
    public class MigratorHost<TContext> : IMigratorHost<TContext> where TContext : IMigrationContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MigratorHost<TContext>> _logger;
        private readonly SortedDictionary<string, Type> _migrations;
        private readonly string _contextName;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="configuration">The configuration to use</param>
        /// <param name="manager">The migration manager</param>
        /// <param name="migrations">The migrations found</param>
        /// <param name="logger">The logger to be used</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MigratorHost(
            IServiceProvider serviceProvider, IConfiguration configuration, IMigrationManager<TContext> manager, 
            IEnumerable<Type> migrations, ILogger<MigratorHost<TContext>> logger)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            if (migrations == null)
                throw new ArgumentNullException(nameof(migrations));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _configuration = configuration;
            _logger = logger;
            _migrations = new SortedDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            foreach (var migration in migrations)
                _migrations.Add(migration.Name, migration);
            _contextName = typeof(TContext).Name;

            ServiceProvider = serviceProvider;
            Manager = manager;
        }

        #region Implementation of IMigratorHost<TContext>

        /// <inheritdoc />
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc />
        public IMigrationManager<TContext> Manager { get; }

        /// <inheritdoc />
        public IEnumerable<string> Migrations => _migrations.Keys;

        /// <inheritdoc />
        public async Task ApplyMigrationsAsync(CancellationToken ct)
        {
            if (_migrations.Count == 0)
            {
                _logger.LogWarning(
                    "The migration collection for '{contextName}' context is empty. Nothing to be done.",
                    _contextName);
                return;
            }

            var lastMigrationId = _migrations.Keys.Last();
            await ApplyMigrationsStoppingAtAsync(lastMigrationId, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task ApplyMigrationsStoppingAtAsync(string migrationId, CancellationToken ct)
        {
            if (migrationId == null)
                throw new ArgumentNullException(nameof(migrationId));
            if (string.IsNullOrWhiteSpace(migrationId))
                throw new ArgumentException("Value cannot be whitespace.", nameof(migrationId));

            _logger.LogDebug(
                "About to apply '{migrationId}' migration for '{contextName}' context", migrationId, _contextName);

            if (!_migrations.ContainsKey(migrationId))
                throw new InvalidOperationException(
                    $"There is no matching migration with the identifier '{migrationId}'");

            var dbMigrations = await Manager.GetAllMigrationsAsync(ct).ConfigureAwait(false);
            if (dbMigrations.Any(e => e.Equals(migrationId, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogDebug(
                    "Migration '{migrationId}' is already applied for '{contextName}' context", migrationId, _contextName);
                return;
            }

            var appliedMigrationIds = new List<string>(dbMigrations.Count);
            appliedMigrationIds.AddRange(dbMigrations);

            //  TODO    Validar match exacto de migrations
        }

        #endregion
    }
}
