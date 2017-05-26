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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Hosting.Handlers
{
    /// <summary>
    /// The parameter to the <see cref="IServiceProvider"/> builder
    /// for the host builder
    /// </summary>
    public class ServiceProviderBuilderParam
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceCollection">The service collection</param>
        /// <param name="factory">The logger factory</param>
        /// <param name="configuration">The configuration</param>
        /// <param name="environment">The hosting environment</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ServiceProviderBuilderParam(IServiceCollection serviceCollection, ILoggerFactory factory, IConfiguration configuration, IHostingEnvironment environment)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (environment == null) throw new ArgumentNullException(nameof(environment));

            ServiceCollection = serviceCollection;
            Factory = factory;
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        /// The service collection
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// The logger factory
        /// </summary>
        public ILoggerFactory Factory { get; }

        /// <summary>
        /// The host builder configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The hosting environment
        /// </summary>
        public IHostingEnvironment Environment { get; }
    }
}