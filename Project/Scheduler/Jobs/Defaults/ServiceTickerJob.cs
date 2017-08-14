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
        public override void Run()
        {
            // throw new DivideByZeroException("Can't divide 9 by 0.");
        }
    }
}