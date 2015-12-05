using System;
using MongoDB.Driver;

namespace MongoDB.Testing.Mongo
{
    public interface IRandomMongoDatabase : IDisposable
    {
        IMongoDatabase Database { get; }
    }
}