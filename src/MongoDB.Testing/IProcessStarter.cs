using System.Diagnostics;
using MongoDB.Testing.Mongo;

namespace MongoDB.Testing
{
    public interface IProcessStarter
    {
        IProcess Start(ProcessStartInfo processStartInfo);
    }
}