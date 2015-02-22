using System;
using MongoDB.Driver;

namespace MongoDB.Testing.Mongo
{
    public class MongoTestServer : IDisposable
    {
        private readonly ushort _port;
        private readonly IRandomMongoDatabase _randomDatabase;
        private readonly IProcess _process;
        private readonly IFileSystem _fileSystem;
        private readonly string _dataPath;
        private bool _disposed;

        internal MongoTestServer(UInt16 port, IProcess process, IFileSystem fileSystem, string dataPath)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }

            if (dataPath == null)
            {
                throw new ArgumentNullException("dataPath");
            }

            _port = port;
            _process = process;
            _fileSystem = fileSystem;
            _dataPath = dataPath;
            _randomDatabase = new DefaultRandomMongoDatabase(port);
        }

        public MongoDatabase Database
        {
            get { return _randomDatabase.Database; }
        }

        /// <summary>
        /// Dispose will terminate the created mongod.exe instance and delete the created temp folder.
        /// </summary>
        public void Dispose()
        {
            if (_disposed == false)
            {
                _randomDatabase.Dispose();
                _process.Kill();
                _process.WaitForExit();

                if (_fileSystem.DirectoryExists(_dataPath))
                {
                    _fileSystem.DeleteDirectory(_dataPath, true);
                }

                _disposed = true;
            }
        }

        public static MongoTestServer Start(UInt16 port)
        {
            var defaultFileSystem = new DefaultFileSystem();
            var mongoExeLocator = new DefaultMongoExeLocator(defaultFileSystem);

            return Start(port, mongoExeLocator, new DefaultProcessStarter(), defaultFileSystem);
        }

        public static MongoTestServer Start(UInt16 port, IMongoExeLocator mongoExeLocator)
        {
            return Start(port, mongoExeLocator, new DefaultProcessStarter(), new DefaultFileSystem());
        }

        internal static MongoTestServer Start(UInt16 port, IMongoExeLocator mongoExeLocator, IProcessStarter processStarter, IFileSystem fileSystem)
        {
            if (mongoExeLocator == null)
            {
                throw new ArgumentNullException("mongoExeLocator");
            }

            if (processStarter == null)
            {
                throw new ArgumentNullException("processStarter");
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }

            var exeFacade = new MongodExeFacade(mongoExeLocator, processStarter);
            string dbPath = fileSystem.CreateTempFolder();
            var mongodExeOptions = new MongodExeOptions(port, dbPath);
            IProcess process = exeFacade.Start(mongodExeOptions);

            return new MongoTestServer(port, process, fileSystem, dbPath);
        }
    }
}