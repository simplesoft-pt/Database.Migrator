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
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// The migration registration builder
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class MigrationsBuilder<TContext> where TContext : IMigrationContext
    {
        private readonly TypeInfo _migrationContextType = typeof(IMigration<TContext>).GetTypeInfo();
        private readonly List<Type> _migrations = new List<Type>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceCollection">The service collection</param>
        public MigrationsBuilder(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// The service collection
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// The collection of migrations types for this context
        /// </summary>
        public IReadOnlyList<Type> Migrations => _migrations;

        /// <summary>
        /// Adds the type as a migration
        /// </summary>
        /// <param name="migrationType">The migration type</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddMigration(Type migrationType)
        {
            if (migrationType == null)
                throw new ArgumentNullException(nameof(migrationType));

            if (_migrations.Exists(type => type == migrationType))
                throw new ArgumentException(
                    $"The is already a migration of type {migrationType}", nameof(migrationType));

            var typeInfo = migrationType.GetTypeInfo();
            if (typeInfo.IsAbstract || !typeInfo.IsClass || !_migrationContextType.IsAssignableFrom(migrationType))
                throw new ArgumentException(
                    $"The type must be a class not abstract and extend from {_migrationContextType}",
                    nameof(migrationType));

            _migrations.Add(migrationType);
        }

        /// <summary>
        /// Adds the type as a migration
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        public void AddMigration<TMigration>() where TMigration : IMigration<TContext>
        {
            AddMigration(typeof(TMigration));
        }
    }
}