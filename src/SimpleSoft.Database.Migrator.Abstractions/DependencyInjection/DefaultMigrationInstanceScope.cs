using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Default instance scope implementation that uses <see cref="Activator"/> for creating the instances.
    /// All instances will be created a single time per scope.
    /// </summary>
    public class DefaultMigrationInstanceScope : IMigrationInstanceScope
    {
        private readonly Dictionary<Type, object> _resolvedInstances = new Dictionary<Type, object>();

        /// <inheritdoc />
        ~DefaultMigrationInstanceScope() => Dispose(false);

        /// <inheritdoc />
        public object GetInstance(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return GetOrAddInstance(type);
        }

        /// <inheritdoc />
        public T GetInstance<T>() => (T) GetOrAddInstance(typeof(T));

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(_resolvedInstances.Count == 0)
                return;

            if (disposing)
            {
                foreach (var instance in _resolvedInstances.Values)
                    (instance as IDisposable)?.Dispose();
            }

            _resolvedInstances.Clear();
        }

        private object GetOrAddInstance(Type type)
        {
            if (_resolvedInstances.TryGetValue(type, out var instance))
                return instance;

            //  TODO    Recursive creation of constructor instances
            instance = Activator.CreateInstance(type);
            _resolvedInstances[type] = instance;

            return instance;
        }
    }
}