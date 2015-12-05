using MongoDB.Testing.Mongo;

namespace MongoDBImplementationSample.Tests
{
    public class MongodExeLocator : IMongoExeLocator
    {
        public string Locate()
        {
            return @"C:\Program Files\MongoDB\Server\3.0\bin\mongod.exe";
        }
    }
}