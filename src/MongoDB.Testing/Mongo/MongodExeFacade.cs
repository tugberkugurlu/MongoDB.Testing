using System;
using System.Diagnostics;

namespace MongoDB.Testing.Mongo
{
    public class MongodExeFacade
    {
        private readonly IMongoExeLocator _exeLocator;
        private readonly IProcessStarter _processStarter;

        public MongodExeFacade(IMongoExeLocator exeLocator, IProcessStarter processStarter)
        {
            if (exeLocator == null)
            {
                throw new ArgumentNullException("exeLocator");
            }

            if (processStarter == null)
            {
                throw new ArgumentNullException("processStarter");
            }

            _exeLocator = exeLocator;
            _processStarter = processStarter;
        }

        public IProcess Start(MongodExeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            string exeLocation = _exeLocator.Locate();
            string exeArgs = options.ToString();
            var processStartInfo = new ProcessStartInfo(exeLocation, exeArgs)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            return _processStarter.Start(processStartInfo);
        }
    }
}