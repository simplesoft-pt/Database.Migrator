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

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Logger used by migrations classes
    /// </summary>
    public interface IMigrationLogger
    {
        /// <summary>
        /// Logs the given log entry
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="exception">An optional exception</param>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        void Log(MigrationLogLevel level, Exception exception, string messageFormat, params string[] args);

        /// <summary>
        /// Checks if a given log level is enabled
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabled(MigrationLogLevel level);

        /// <summary>
        /// Creates a logger scope with the given information.
        /// </summary>
        /// <param name="messageFormat">The message format</param>
        /// <param name="args">An optional collection of message format arguments</param>
        /// <returns>The disposable logger scope</returns>
        IDisposable Scope(string messageFormat, params string[] args);
    }
}
