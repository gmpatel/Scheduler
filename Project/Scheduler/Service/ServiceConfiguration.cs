using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Service;
using Scheduler.Service.Interfaces;
using Topshelf;

namespace Scheduler.Service
{
    internal static class ServiceConfiguration
    {
        internal static void Configure()
        {

            HostFactory.Run(configure =>
            {
                configure.Service<ISchedulerService>(service =>
                {
                    service.ConstructUsing(s => new SchedulerService());

                    service.WhenStarted(s => s.Start());
                    service.WhenContinued(s => s.Start());

                    service.WhenPaused(s => s.Stop());
                    service.WhenStopped(s => s.Stop());
                    service.WhenShutdown(s => s.Stop());
                });
                 
                configure.RunAsLocalSystem();
                configure.SetServiceName("Scheduler Service");
                configure.SetDisplayName("Scheduler Service");
                configure.SetDescription("Scheduler Service - Allows code and configure plugins to schedule automated tasks");
            });
        }
    }
}  