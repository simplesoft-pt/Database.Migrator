using System;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Extensions for <see cref="IMigrationInstanceScope"/> instances.
    /// </summary>
    public static class MigrationInstanceScopeExtensions
    {
        /// <summary>
        /// Gets a required instance for the given type.
        /// </summary>
        /// <param name="scope">The instance scope</param>
        /// <param name="type">The instance type</param>
        /// <returns>The resolved instance</returns>
        /// <exception cref="RequiredInstanceIsNullException"></exception>
        public static object GetRequiredInstance(this IMigrationInstanceScope scope, Type type)
        {
            var instance = scope.GetInstance(type);
            if (instance == null)
                throw new RequiredInstanceIsNullException(type);

            return instance;
        }

        /// <summary>
        /// Gets a required instance for the given type.
        /// </summary>
        /// <typeparam name="T">The instance type</typeparam>
        /// <param name="scope">The instance scope</param>
        /// <returns>The resolved instance</returns>
        /// <exception cref="RequiredInstanceIsNullException"></exception>
        public static T GetRequiredInstance<T>(this IMigrationInstanceScope scope) =>
            (T) scope.GetInstance(typeof(T));
    }
}