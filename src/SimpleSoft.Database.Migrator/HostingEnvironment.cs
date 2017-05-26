using Microsoft.Extensions.FileProviders;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Provides information about the hosting environment an application is running in.
    /// </summary>
    public class HostingEnvironment : IHostingEnvironment
    {
        #region Implementation of IHostingEnvironment

        /// <inheritdoc />
        public string ApplicationName { get; set; }

        /// <inheritdoc />
        public IFileProvider ContentRootFileProvider { get; set; }

        /// <inheritdoc />
        public string ContentRootPath { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        #endregion
    }
}