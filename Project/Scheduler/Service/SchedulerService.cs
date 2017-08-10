using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using System.IO;
using Scheduler.Configuration;
using Scheduler.Core;
using Scheduler.Quartz;

namespace Scheduler.Service
{
    public class SchedulerService
    {
        public void Start()
        {
            Bootstrapper.Start(CustomConfig.Instance);
        }
        public void Stop()
        {
            Bootstrapper.Stop();
        }
    }
}