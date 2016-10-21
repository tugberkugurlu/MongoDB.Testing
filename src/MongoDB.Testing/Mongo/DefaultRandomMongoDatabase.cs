using System;
using System.Collections.Generic;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

namespace MongoDB.Testing.Mongo
{
    internal class DefaultRandomMongoDatabase : IRandomMongoDatabase
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;
        private bool _disposed;

        public DefaultRandomMongoDatabase(UInt16 port)
        {
            if(port == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port), port, "The port number cannot be '0'.");
            }

            var databaseName = Guid.NewGuid().ToString("N");
            _client = new MongoClient($"mongodb://localhost:{port}");
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database => _database;

        public void Dispose()
        {
            if (_disposed == false)
            {
                _client.DropDatabaseAsync(_database.DatabaseNamespace.DatabaseName).Wait();
                _client.Cluster.Dispose();

                // Giant hack to simulate old MongoServer.Disconnect() API behaviour.
                // Also see: https://github.com/mongodb/mongo-csharp-driver/blob/ed5c0b3c365e4ed1a1b94a0e1e630e7c9fd5a236/src/MongoDB.Driver/ClusterRegistry.cs
                // TODO: do not clear, remove: https://github.com/mongodb/mongo-csharp-driver/blob/ed5c0b3c365e4ed1a1b94a0e1e630e7c9fd5a236/src/MongoDB.Driver/MongoClient.cs#L52
                var clusterKeyType = typeof (MongoClient).GetTypeInfo().Assembly.GetType("MongoDB.Driver.ClusterKey");
                var clusterRegistryType = typeof (MongoClient).GetTypeInfo().Assembly.GetType("MongoDB.Driver.ClusterRegistry");
                var staticClusterRegistryField = clusterRegistryType.GetTypeInfo().GetField("__instance", BindingFlags.NonPublic | BindingFlags.Static);
                var registryDictionaryField = clusterRegistryType.GetTypeInfo().GetField("_registry", BindingFlags.NonPublic | BindingFlags.Instance);
                var clusterRegistry = staticClusterRegistryField.GetValue(null);
                var registryDictionary = registryDictionaryField.GetValue(clusterRegistry);
                var registeryDictionaryType = typeof(Dictionary<,>).MakeGenericType(clusterKeyType, typeof(ICluster));
                var clearMethodInfo = registeryDictionaryType.GetTypeInfo().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance);
                clearMethodInfo.Invoke(registryDictionary, null);

                _disposed = true;
            }
        }
    }
}