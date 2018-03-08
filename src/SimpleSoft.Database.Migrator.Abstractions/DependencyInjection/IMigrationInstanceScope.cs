using System;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Scope used to resolve instances
    /// </summary>
    public interface IMigrationInstanceScope : IDisposable
    {
        /// <summary>
        /// Gets an instance for the given type.
        /// </summary>
        /// <param name="type">The instance type</param>
        /// <returns>The resolved instance</returns>
        object GetInstance(Type type);

        /// <summary>
        /// Gets an instance for the given type.
        /// </summary>
        /// <typeparam name="T">The instance type</typeparam>
        /// <returns>The resolved instance</returns>
        T GetInstance<T>();
    }
}
