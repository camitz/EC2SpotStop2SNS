using System.ServiceProcess;
using log4net;

namespace SpotStop2SNS
{
    public partial class SpotStop2SNSService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SpotStop2SNSService));

        Worker worker;

        public SpotStop2SNSService()
        {
        }

        protected override void OnStart(string[] args)
        {
            Log.Debug("Start command recieved..");
            worker = new Worker();
        }

        protected override void OnStop()
        {
            worker = null;
        }
    }
}
