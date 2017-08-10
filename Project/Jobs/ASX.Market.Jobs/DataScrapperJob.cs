using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Core;

namespace ASX.Market.Jobs
{
    public class DataScrapperJob : SchedulerJobBase
    {
        public override void Run()
        {
            var tickLogText = string.Format("{0} - ASX Market Data Scrapper Job : Scrapper working round the clock", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt"));
            File.WriteAllText("ASX.Market.Jobs.DataScrapperJob.log", tickLogText);
            Console.WriteLine(tickLogText);
        }
    }
}