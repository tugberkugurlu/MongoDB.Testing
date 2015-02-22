using System;
using System.IO;

namespace MongoDB.Testing
{
    internal class DefaultFileSystem : IFileSystem
    {
        /// <summary>
        /// Creates a temp folder under the current user's temporary folder and returns it's path.
        /// </summary>
        /// <returns>The absolute path of the created folder.</returns>
        public string CreateTempFolder()
        {
            var tempFolderPath = Path.GetTempPath();
            var dbPath = Path.Combine(tempFolderPath, Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dbPath);

            return dbPath;
        }

        public bool FileExists(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return Directory.Exists(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }
    }
}