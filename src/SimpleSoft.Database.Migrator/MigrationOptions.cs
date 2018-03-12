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

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Options for a migration context.
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    public class MigrationOptions<TContext> where TContext : IMigrationContext
    {
        private static readonly TypeInfo MigrationType = typeof(IMigration<TContext>).GetTypeInfo();
#if NET451
        private readonly List<Type> _migrationTypes = new List<Type>();
#else
        private readonly HashSet<Type> _migrationTypes = new HashSet<Type>();
#endif

        /// <summary>
        /// The context name.
        /// Defaults to context class name.
        /// </summary>
        public string ContextName { get; set; } = typeof(TContext).Name;

        /// <summary>
        /// The collection of known migrations.
        /// </summary>
        public IReadOnlyCollection<Type> MigrationTypes => _migrationTypes;

        /// <summary>
        /// Adds the migration to the collection.
        /// </summary>
        /// <typeparam name="TMigration">The migration type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddMigration<TMigration>() where TMigration : IMigration<TContext>
        {
            var type = typeof(TMigration);
            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsClass || typeInfo.IsAbstract)
                throw new ArgumentException(
                    $"The migration of type '{type.FullName}' must be a not abstract class", nameof(type));

            if (_migrationTypes.Contains(type))
                throw new InvalidOperationException(
                    $"The migration of type '{type.FullName}' is already registered");

            _migrationTypes.Add(type);
        }

        /// <summary>
        /// Adds the migration to the collection.
        /// </summary>
        /// <param name="type">The migration type</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddMigration(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsClass || typeInfo.IsAbstract)
                throw new ArgumentException(
                    $"The migration of type '{type.FullName}' must be a not abstract class", nameof(type));

            if (!MigrationType.IsAssignableFrom(typeInfo))
                throw new ArgumentException(
                    $"The migration of type '{type.FullName}' must be assignable to {MigrationType.FullName}", nameof(type));

            if (_migrationTypes.Contains(type))
                throw new InvalidOperationException(
                    $"The migration of type '{type.FullName}' is already registered");

            _migrationTypes.Add(type);
        }
    }
}
