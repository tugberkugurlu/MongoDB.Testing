using MongoDB.Testing.Mongo;

namespace ConsoleSample
{
    public class MongodExeLocator : IMongoExeLocator
    {
        public string Locate()
        {
            return @"c:\mongo\bin\mongod.exe";
        }
    }
}