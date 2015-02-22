using System;

namespace MongoDB.Testing
{
    /// <summary>
    /// Relevant command line arguments of mongod.exe 
    /// </summary>
    /// <remarks>
    /// For more information see http://docs.mongodb.org/manual/reference/program/mongod/ and 
    /// http://docs.mongodb.org/v2.6/reference/program/mongod.exe/.
    /// </remarks>
    public class MongodExeOptions
    {
        private readonly UInt16 _port;
        private readonly string _dbPath;

        public MongodExeOptions(UInt16 port, string dbPath)
        {
            if (port == 0)
            {
                throw new ArgumentOutOfRangeException("port", port, "The port number cannot be '0'.");
            }

            _port = port;
            _dbPath = dbPath;
        }

        public override string ToString()
        {
            return string.Format("--port {0} --dbpath {1}", _port, _dbPath);
        }
    }
}