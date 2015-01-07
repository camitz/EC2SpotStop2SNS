using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Timers;
using Amazon;
using Amazon.SimpleNotificationService.Model;
using log4net;

namespace SpotStop2SNS
{

    public class Worker
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Worker));
        private Timer _timer;

        private const double InitialDelay = 1000;
        private const double ShortInterval = 5000;
        private string Topic = ConfigurationManager.AppSettings["Topic"] ?? "SpotInstanceTermination";

        private bool _marked = false;

        private object _lock = new Object();
        private string _arn;

        public Worker()
        {
            Log.Debug("Starting worker.");


            var client = AWSClientFactory.CreateAmazonSimpleNotificationServiceClient();

            Topic = string.Format(Topic, AWSConfigs.AWSRegion);

            var topic = client.ListTopics().Topics.FirstOrDefault(x => x.TopicArn.Contains(Topic));
            if (topic == null)
            {
                Log.Debug("Creating topic " + Topic);

                _arn = client.CreateTopic(Topic).TopicArn;
            }
            else
            {
                _arn = topic.TopicArn;
            }

            _timer = new Timer(InitialDelay);
            GC.KeepAlive(_timer);

            _timer.Elapsed += this.Tick;

            _timer.Start();

            Log.Debug("Timer started.");
        }



        public void Tick(object sender, EventArgs args)
        {
            lock (_lock)
            {
                Log.Debug("Tick.");

                string s;
                try
                {
                    s = new WebClient().DownloadString(new Uri("http://169.254.169.254/latest/meta-data/spot/termination-time"));
                }
                catch (WebException e)
                {
                    Log.Debug("Exception caught (normal).", e);
                    return;
                }

                Log.Debug("String caught: " + s);

                _timer.Stop();

                var id = new WebClient().DownloadString(new Uri("http://169.254.169.254/latest/meta-data/instance-id"));

                var msg = string.Format("Instance {0} marked for termination at {1}.", id, s);

                Log.Debug("Publishing:" + msg);

                var client = AWSClientFactory.CreateAmazonSimpleNotificationServiceClient();
                client.Publish(_arn, msg);

                Log.Debug("Ending");
            }
        }
    }
}
