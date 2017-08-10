using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Scheduler.Core;

namespace Scheduler.Jobs.Defaults
{
    public class ServiceTickerJob : SchedulerJobBase
    {
        private readonly object locker = new object();

        public override void Run()
        {
            lock (locker)
            {
                var tickLogText = string.Format("{0} - Ticker Job : Schedular Service is alive and working round the clock", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt"));
                File.WriteAllText("Scheduler.Jobs.Defaults.ServiceTickerJob.log", tickLogText);
                Console.WriteLine(tickLogText);
            }
        }
    }
}