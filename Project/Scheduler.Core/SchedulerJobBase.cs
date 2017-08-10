using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Scheduler.Core
{
    public abstract class SchedulerJobBase : ISchedulerJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Run();
        }

        public abstract void Run();
    }
}