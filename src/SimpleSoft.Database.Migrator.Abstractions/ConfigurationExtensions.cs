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
using Microsoft.Extensions.Configuration;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extension methods for <see cref="IMigratorHostBuilder"/>
    /// </summary>
    public static class ConfigurationExtensions
    {
        #region Environment

        /// <summary>
        /// Gets the environment value from the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <returns>The environment value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Environment(this IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration[MigratorHostDefaults.EnvironmentKey];
        }

        /// <summary>
        /// Sets the environment value into the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <param name="environment">The environment value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void Environment(this IConfiguration configuration, string environment)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (environment == null) throw new ArgumentNullException(nameof(environment));
            if (string.IsNullOrWhiteSpace(environment))
                throw new ArgumentException("Value cannot be whitespace.", nameof(environment));

            configuration[MigratorHostDefaults.EnvironmentKey] = environment;
        }

        #endregion

        #region ContentRoot

        /// <summary>
        /// Gets the content root value from the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <returns>The environment value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ContentRoot(this IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration[MigratorHostDefaults.ContentRootKey];
        }

        /// <summary>
        /// Sets the content root value into the given configuration
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        /// <param name="contentRoot">The content root value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ContentRoot(this IConfiguration configuration, string contentRoot)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            configuration[MigratorHostDefaults.ContentRootKey] = contentRoot;
        }

        #endregion
    }
}
