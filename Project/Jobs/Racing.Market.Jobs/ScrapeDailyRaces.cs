using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Core;

namespace Racing.Market.Jobs
{
    public class ScrapeDailyRaces : SchedulerJobBase
    {
        private readonly string fileName;

        public ScrapeDailyRaces()
        {
            this.fileName = string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss}", this.GetType().FullName, DateTime.Now, DateTime.Now);
        }
        
        public override void Run()
        {
            
        }
    }
}