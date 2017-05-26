using Microsoft.Extensions.FileProviders;

namespace SimpleSoft.Database.Migrator
{
    /// <summary>
    /// Provides information about the hosting environment an application is running in.
    /// </summary>
    public interface IHostingEnvironment
    {
        /// <summary>
        /// The application name. 
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// The <see cref="IFileProvider"/> pointing at <see cref="ContentRootPath"/>.
        /// </summary>
        IFileProvider ContentRootFileProvider { get; set; }

        /// <summary>
        /// The absolute path to the directory that contains the application content files.
        /// </summary>
        string ContentRootPath { get; set; }

        /// <summary>
        /// The environment name.
        /// </summary>
        string Name { get; set; }
    }
}
