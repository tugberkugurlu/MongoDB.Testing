namespace MongoDB.Testing.Mongo
{
    public interface IMongoExeLocator
    {
        /// <exception cref="MongodExeNotFoundException">Thrown when the mongod.exe cannot be located.</exception>
        string Locate();
    }
}