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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extensions to help register services into the migrator
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds support for migrations for the given <see cref="IMigrationContext"/>.
        /// </summary>
        /// <typeparam name="TContext">The migration context</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="config">Configuration handler</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMigrations<TContext>(
            this IServiceCollection services, Action<MigrationsBuilder<TContext>> config)
            where TContext : class, IMigrationContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (config == null) throw new ArgumentNullException(nameof(config));

            var builder = new MigrationsBuilder<TContext>(services);
            config(builder);

            services.TryAddSingleton<INamingNormalizer<TContext>>(new DefaultNamingNormalizer<TContext>());

            services.TryAddScoped<TContext>();
            foreach (var migrationType in builder.Migrations)
            {
                services.TryAddScoped(migrationType);
            }
            services.TryAddSingleton<IEnumerable<MigrationMetadata<TContext>>>(k =>
            {
                var normalizer = k.GetRequiredService<INamingNormalizer<TContext>>();

                var list = new List<MigrationMetadata<TContext>>(builder.Migrations.Count);
                list.AddRange(builder.Migrations.Select(e => new MigrationMetadata<TContext>(
                    normalizer.Normalize(e.Name), normalizer.Normalize(e.FullName), e)));

                return list;
            });
            services.TryAddScoped<IMigrationRunner<TContext>, MigrationRunner<TContext>>();

            return services;
        }
    }
}
