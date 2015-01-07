using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace Coin.Diagnostics
{
    public enum TraceLevel { Level1, Level2, Level3 }

    public class MyTrace
    {
        static MyTrace _instance;
        TraceLevel TraceLevel = TraceLevel.Level1;

        private static Object _thisLock = new Object();

        private static MyTrace Instance
        {
            get
            {
            lock (_thisLock)
            {
                if (_instance == null)
                    _instance = new MyTrace();
            }
            return _instance;
        }
        }

        public MyTrace()
        {
            if (ConfigurationManager.AppSettings["TracePath"] != null) 
            {
                // Creates the text file that the trace listener will write to.
                System.IO.FileStream myTraceLog = new
                   System.IO.FileStream(
                       Path.Combine(ConfigurationManager.AppSettings["TracePath"], "EmailQueueWorkerTrace"),
                       System.IO.FileMode.OpenOrCreate);

                // Creates the new trace listener.
                TextWriterTraceListener myListener =
                   new TextWriterTraceListener(myTraceLog);

                Trace.Listeners.Add(myListener);
            }

            if (ConfigurationManager.AppSettings["TraceToConsole"] != null && Boolean.Parse(ConfigurationManager.AppSettings["TraceToConsole"]) == true)
            {
                // Creates the new trace listener.
                TextWriterTraceListener myListener =
                   new TextWriterTraceListener(Console.Out);

                Trace.Listeners.Add(myListener);
            }

            if (ConfigurationManager.AppSettings["TraceLevel"] != null)
            {
                TraceLevel = (TraceLevel)Enum.Parse(typeof(TraceLevel), ConfigurationManager.AppSettings["TraceLevel"]);
            }
        }

        public static void WriteLine(TraceLevel level, object value)
        {
            string s = String.Format("{0}\t{1}\t", DateTime.Now, Thread.CurrentThread.Name) + value;
            Trace.WriteLineIf(level <= Instance.TraceLevel,
                value);
            Trace.Flush();
        }

        public static void WriteLine(object value)
        {
            WriteLine(TraceLevel.Level1, value);
        }

        static SpecificLevelTrace _level1Trace;
        public static SpecificLevelTrace Level1Trace
        {
            get
            {
                lock (_thisLock)
                {
                    if (_level1Trace == null)
                        _level1Trace = new SpecificLevelTrace(TraceLevel.Level1);
                }
                return _level1Trace;
            }
        }
        static SpecificLevelTrace _level2Trace;
        public static SpecificLevelTrace Level2Trace
        {
            get
            {
                lock (_thisLock)
                {
                    if (_level2Trace == null)
                        _level2Trace = new SpecificLevelTrace(TraceLevel.Level2);
                }
                return _level2Trace;
            }
        }
        static SpecificLevelTrace _level3Trace;
        public static SpecificLevelTrace Level3Trace
        {
            get
            {
                lock (_thisLock)
                {
                    if (_level3Trace == null)
                        _level3Trace = new SpecificLevelTrace(TraceLevel.Level3);
                }
                return _level3Trace;
            }
        }

        public static void Flush()
        {
            Trace.Flush();
        }
    }

    public class SpecificLevelTrace
    {
        TraceLevel _level;

        public SpecificLevelTrace(TraceLevel level)
        {
            _level = level;
        }

        public void WriteLine(object value)
        {
            MyTrace.WriteLine(_level, value);
        }
    }


}
