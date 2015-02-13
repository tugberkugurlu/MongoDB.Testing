using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Testing
{
    public class MongoTestServer : IDisposable
    {
        public MongoTestServer()
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class MongoExeWrapper
    {
    }

    public interface IMongoExeLocator
    {
        string Locate();
    }

    public class MongoExeLocator : IMongoExeLocator
    {
        public string Locate()
        {
            throw new NotImplementedException();
        }
    }
}
