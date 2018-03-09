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

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Medadata used to identify a migration
    /// </summary>
    public sealed class MigrationNormalizedMeta
    {
        private MigrationNormalizedMeta(string id, string className, Type type)
        {
            Id = id;
            ClassName = className;
            Type = type;
        }

        /// <summary>
        /// The migration unique identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The migration class name
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// The migration type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Builds a normalized migration metadata for the given type
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="normalizer">The normalizer to be used</param>
        /// <param name="type">The migration type</param>
        /// <returns>Normalized migration metadata instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MigrationNormalizedMeta Build<TContext>(INamingNormalizer<TContext> normalizer, Type type)
            where TContext : IMigrationContext
        {
            if (normalizer == null) throw new ArgumentNullException(nameof(normalizer));
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new MigrationNormalizedMeta(
                normalizer.Normalize(type.Name),
                normalizer.Normalize(type.FullName),
                type);
        }
    }
}