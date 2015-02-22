using System;
using MongoDB.Driver;

namespace MongoDB.Testing.Mongo
{
    internal class DefaultRandomMongoDatabase : IRandomMongoDatabase
    {
        private readonly MongoDatabase _database;
        private readonly MongoServer _server;
        private bool _disposed;

        public DefaultRandomMongoDatabase(UInt16 port)
        {
            if(port == 0)
            {
                throw new ArgumentOutOfRangeException("port", port, "The port number cannot be '0'.");
            }

            var databaseName = Guid.NewGuid().ToString("N");
            var client = new MongoClient(string.Format("mongodb://localhost:{0}", port));
            _server = client.GetServer();
            _database = _server.GetDatabase(databaseName);
        }

        public MongoDatabase Database
        {
            get { return _database; }
        }

        public void Dispose()
        {
            if (_disposed == false)
            {
                _database.Drop();
                _server.Disconnect();
                _disposed = true;
            }
        }
    }
}