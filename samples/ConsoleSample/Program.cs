using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Testing.Mongo;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var locator = new MongodExeLocator();
            using (var server = MongoTestServer.Start(27017, locator))
            {
            }
        }
    }
}
