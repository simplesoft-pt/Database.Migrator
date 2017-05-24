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
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extensions for <see cref="MigrationsBuilder{TContext}"/> instances.
    /// </summary>
    public static class SqlServerMigrationsBuilderExtensions
    {
        /// <summary>
        /// Adds SQL Server support for the given <see cref="IRelationalMigrationContext"/>.
        /// </summary>
        /// <typeparam name="TContext">The context type</typeparam>
        /// <param name="builder">The migration builder</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="config">An optional configuration handler</param>
        /// <returns>The builder after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static MigrationsBuilder<TContext> AddSqlServer<TContext>(
            this MigrationsBuilder<TContext> builder, string connectionString, Action<SqlServerContextOptions<TContext>> config = null)
            where TContext : class, IRelationalMigrationContext
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var options = new SqlServerContextOptions<TContext>(connectionString);
            config?.Invoke(options);
            builder.ServiceCollection.AddSingleton(k => options);
            builder.ServiceCollection.AddSingleton<SqlServerContextOptions>(
                k => k.GetRequiredService<SqlServerContextOptions<TContext>>());

            builder.ServiceCollection.AddScoped<SqlServerMigrationManager<TContext>>();
            builder.ServiceCollection.AddScoped<ISqlServerMigrationManager<TContext>>(
                k => k.GetRequiredService<SqlServerMigrationManager<TContext>>());
            builder.ServiceCollection.AddScoped<IMigrationManager<TContext>>(
                k => k.GetRequiredService<SqlServerMigrationManager<TContext>>());
            
            return builder;
        }
    }
}
