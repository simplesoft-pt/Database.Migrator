using System;

// ReSharper disable once CheckNamespace
namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Thrown when resolving an instance returns null
    /// </summary>
    public class RequiredInstanceIsNullException : InvalidOperationException
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="instanceType">The type beeing resolved</param>
        public RequiredInstanceIsNullException(Type instanceType)
            : base($"Failed to get a required instance for type '{instanceType.FullName}'")
        {
            InstanceType = instanceType;
        }

        /// <summary>
        /// The instance type beeing resolved
        /// </summary>
        public Type InstanceType { get; }
    }
}