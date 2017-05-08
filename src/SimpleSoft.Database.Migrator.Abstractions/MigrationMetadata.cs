using System;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Medadata used to identify a migration
    /// </summary>
    public sealed class MigrationMetadata<TContext> where TContext : IMigrationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="id">The migration name</param>
        /// <param name="className">The migration class name</param>
        /// <param name="type">The migration type</param>
        public MigrationMetadata(string id, string className, Type type)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (className == null)
                throw new ArgumentNullException(nameof(className));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value cannot be whitespace.", nameof(id));
            if (string.IsNullOrWhiteSpace(className))
                throw new ArgumentException("Value cannot be whitespace.", nameof(className));

            Id = id;
            ClassName = className;
            Type = type;
        }

        /// <summary>
        /// The migration name
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
    }
}