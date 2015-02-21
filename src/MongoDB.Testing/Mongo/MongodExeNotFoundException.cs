using System;

namespace MongoDB.Testing.Mongo
{
    public class MongodExeNotFoundException : Exception
    {
        /// <summary>
        /// Exception to be thrown when the mongod.exe cannot be found.
        /// </summary>
        /// <param name="paths">The paths that have been looked at but don't contain mongod.exe</param>
        public MongodExeNotFoundException(string[] paths) : base("mongod.exe cannot be found under: " + string.Join(", ", paths))
        {
        }
    }
}
