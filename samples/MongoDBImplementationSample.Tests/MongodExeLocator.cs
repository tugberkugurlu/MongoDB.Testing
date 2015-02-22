using MongoDB.Testing.Mongo;

namespace MongoDBImplementationSample.Tests
{
    public class MongodExeLocator : IMongoExeLocator
    {
        public string Locate()
        {
            return @"c:\mongo\bin\mongod.exe";
        }
    }
}