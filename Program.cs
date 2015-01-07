using System;
using System.ServiceProcess;
using System.Threading;
using log4net;

namespace SpotStop2SNS
{
    static class Program
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            if (args.Length > 0 && args[0] == "-Start")
            {
                Log.Debug("Starting from commandline.");
                new Worker();

                while (true)
                    Thread.Sleep(3000);
            }
            else
            {
                //int p = 0;
                //while (p==0)
                //    Thread.Sleep(3000);

                Log.Debug("Starting as service.");
                try
                {
                    ServiceBase.Run(new ServiceBase[] { new SpotStop2SNSService() });
                }
                catch (Exception e)
                {
                    Log.Debug("Exception.", e);
                }
            }
        }
    }
}
