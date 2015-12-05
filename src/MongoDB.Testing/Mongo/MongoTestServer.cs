using System;
using MongoDB.Driver;

namespace MongoDB.Testing.Mongo
{
    public class MongoTestServer : IDisposable
    {
        private readonly IRandomMongoDatabase _randomDatabase;
        private readonly IProcess _process;
        private readonly IFileSystem _fileSystem;
        private readonly string _dataPath;
        private readonly MongoTestServerMode _mode;
        private bool _disposed;

        /// <summary>
        /// Constructs a <see cref="MongoTestServer" /> instance without starting a new process.
        /// </summary>
        /// <param name="port">The mongod.exe instance port number.</param>
        internal MongoTestServer(UInt16 port)
        {
            _randomDatabase = new DefaultRandomMongoDatabase(port);
            _mode = MongoTestServerMode.WithoutOwnedProcess;
        }

        /// <summary>
        /// Constructs a <see cref="MongoTestServer" /> instance by starting a new process.
        /// </summary>
        /// <param name="port">The mongod.exe instance port number.</param>
        /// <param name="process"></param>
        /// <param name="fileSystem"></param>
        /// <param name="dataPath"></param>
        internal MongoTestServer(UInt16 port, IProcess process, IFileSystem fileSystem, string dataPath)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (dataPath == null)
            {
                throw new ArgumentNullException(nameof(dataPath));
            }

            _process = process;
            _fileSystem = fileSystem;
            _dataPath = dataPath;
            _randomDatabase = new DefaultRandomMongoDatabase(port);
            _mode = MongoTestServerMode.WithOwnedProcess;
        }

        public IMongoDatabase Database => _randomDatabase.Database;

        /// <summary>
        /// Terminates the created mongod.exe instance and deletes the created temp folder if the instance is 
        /// created through <see cref="Start(ushort)" /> or <see cref="Start(ushort, IMongoExeLocator)"/>. If the instance 
        /// is created through <see cref="Setup" />, the initialized <see cref="IRandomMongoDatabase" /> 
        /// implementation instance will be dropped.
        /// </summary>
        public void Dispose()
        {
            if (_disposed == false)
            {
                _randomDatabase.Dispose();

                if (_mode == MongoTestServerMode.WithOwnedProcess)
                {
                    _process.Kill();
                    _process.WaitForExit();

                    if (_fileSystem.DirectoryExists(_dataPath))
                    {
                        _fileSystem.DeleteDirectory(_dataPath, true);
                    }
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

        /// <summary>
        /// Sets up a <see cref="MongoTestServer" /> instance. Use this static method if you already have 
        /// a mongod.exe instance running with a known port number.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static MongoTestServer Setup(UInt16 port)
        {
            return new MongoTestServer(port);
        }

        internal static MongoTestServer Start(UInt16 port, IMongoExeLocator mongoExeLocator, IProcessStarter processStarter, IFileSystem fileSystem)
        {
            if (mongoExeLocator == null)
            {
                throw new ArgumentNullException(nameof(mongoExeLocator));
            }

            if (processStarter == null)
            {
                throw new ArgumentNullException(nameof(processStarter));
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            var exeFacade = new MongodExeFacade(mongoExeLocator, processStarter);
            string dbPath = fileSystem.CreateTempFolder();
            var mongodExeOptions = new MongodExeOptions(port, dbPath);
            IProcess process = exeFacade.Start(mongodExeOptions);

            return new MongoTestServer(port, process, fileSystem, dbPath);
        }

        private enum MongoTestServerMode : byte
        {
            WithOwnedProcess = 1,
            WithoutOwnedProcess = 2
        }
    }
}