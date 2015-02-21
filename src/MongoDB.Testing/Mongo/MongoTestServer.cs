using System;
using System.IO;

namespace MongoDB.Testing.Mongo
{
    public class MongoTestServer : IDisposable
    {
        private readonly IProcess _process;
        private readonly string _dataPath;
        private bool _disposed;

        private MongoTestServer(IProcess process, string dataPath)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }

            if (dataPath == null)
            {
                throw new ArgumentNullException("dataPath");
            }

            _process = process;
            _dataPath = dataPath;
        }

        /// <summary>
        /// Dispose will terminate the created mongod.exe instance and delete the created temp folder.
        /// </summary>
        public void Dispose()
        {
            if (_disposed == false)
            {
                _process.Kill();

                if (Directory.Exists(_dataPath))
                {
                    Directory.Delete(_dataPath, true);
                }

                _disposed = true;
            }
        }

        public static MongoTestServer Start()
        {
            var defaultFileSystem = new DefaultFileSystem();
            var mongoExeLocator = new DefaultMongoExeLocator(defaultFileSystem);
            return Start(mongoExeLocator, new DefaultProcessStarter(), defaultFileSystem);
        }

        public static MongoTestServer Start(IMongoExeLocator mongoExeLocator)
        {
            return Start(mongoExeLocator, new DefaultProcessStarter(), new DefaultFileSystem());
        }

        internal static MongoTestServer Start(IMongoExeLocator mongoExeLocator, IProcessStarter processStarter, IFileSystem fileSystem)
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
            var mongodExeOptions = new MongodExeOptions(27017, dbPath);
            IProcess process = exeFacade.Start(mongodExeOptions);

            return new MongoTestServer(process, dbPath);
        }
    }
}