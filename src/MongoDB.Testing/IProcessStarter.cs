using System.Diagnostics;

namespace MongoDB.Testing
{
    internal interface IProcessStarter
    {
        IProcess Start(ProcessStartInfo processStartInfo);
    }
}