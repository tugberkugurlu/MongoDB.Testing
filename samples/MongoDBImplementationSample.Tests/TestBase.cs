using MongoDB.Testing.Mongo;

namespace MongoDBImplementationSample.Tests
{
    public abstract class TestBase
    {
        private const int Port = 27017;

        protected MongoTestServer StartServer()
        {
            return MongoTestServer.Start(Port, new MongodExeLocator());
        }

        protected MongoTestServer SetupServer()
        {
            return MongoTestServer.Setup(Port);
        }
    }
}
