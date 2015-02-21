using System.Diagnostics;

namespace MongoDB.Testing
{
    public interface IProcess
    {
        int Id { get; }
        ProcessStartInfo StartInfo { get; }
        void Kill();
    }
}