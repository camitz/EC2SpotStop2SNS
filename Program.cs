using System;
using System.ServiceProcess;
using System.Threading;
using System.Diagnostics;
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
                ServiceBase[] ServicesToRun;
                try
                {
                    ServicesToRun = new ServiceBase[] 
			            { 
     				            new SpotStop2SNSService()
			            };

                    ServiceBase.Run(ServicesToRun);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
        }
    }
}
