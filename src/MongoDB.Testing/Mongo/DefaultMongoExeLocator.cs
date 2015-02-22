using System;
using System.IO;
using System.Linq;

namespace MongoDB.Testing.Mongo
{
    internal class DefaultMongoExeLocator : IMongoExeLocator
    {
        private const string MongodExeName = "mongod.exe";
        private static readonly string[] PossiblePaths = new[]
        {
            Path.GetFullPath(@"mongo\bin"),
            Path.GetFullPath(@"..\..\..\..\tools\mongo\bin")
        };

        private readonly IFileSystem _fileSystem;

        public DefaultMongoExeLocator(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }

            _fileSystem = fileSystem;
        }

        public string Locate()
        {
            string foundPath = PossiblePaths.FirstOrDefault(path => 
                _fileSystem.FileExists(Path.Combine(path, MongodExeName)));

            if (foundPath == null)
            {
                throw new MongodExeNotFoundException(PossiblePaths);
            }

            return foundPath;
        }
    }
}
