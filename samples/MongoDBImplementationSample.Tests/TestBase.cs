using MongoDB.Testing.Mongo;

namespace MongoDBImplementationSample.Tests
{
    public abstract class TestBase
    {
        protected MongoTestServer StartServer()
        {
            return MongoTestServer.Start(27017, new MongodExeLocator());
        }

        protected IRandomMongoDatabase GetDatabase()
        {
            return new DefaultRandomMongoDatabase(27017);
        }
    }
}
