using System;
using System.Diagnostics;
using MongoDB.Testing.Mongo;

namespace MongoDB.Testing
{
    public class DefaultProcessStarter : IProcessStarter
    {
        public IProcess Start(ProcessStartInfo processStartInfo)
        {
            if (processStartInfo == null)
            {
                throw new ArgumentNullException("processStartInfo");
            }

            Process process = new Process
            {
                StartInfo = processStartInfo
            };

            process.ErrorDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
            process.OutputDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process.AsIProcess();
        }
    }
}