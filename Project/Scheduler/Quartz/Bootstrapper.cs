using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Scheduler.Configuration;
using Scheduler.Configuration.Elements;
using Scheduler.Jobs.Defaults;

namespace Scheduler.Quartz
{
    public class Bootstrapper
    {
        private static IScheduler scheduler;

        public static void Start(CustomConfig config)
        {
            var schedFact = new StdSchedulerFactory();
            scheduler = schedFact.GetScheduler();

            scheduler.Start();

            var builtInJob = JobBuilder.Create<ServiceTickerJob>()
                .WithIdentity(string.Format("serviceTickerJob {0}", Guid.NewGuid()), "Group")
                .Build();

            var builtInJobTrigger = TriggerBuilder.Create()
                .WithIdentity(string.Format("ServiceTickerTrigger {0}", Guid.NewGuid()), "Group")
                .WithCronSchedule("0 * * * * ?")
                .Build();

            scheduler.ScheduleJob(builtInJob, builtInJobTrigger);

            // if (Scope.Args != null && Scope.Args.Any() && Scope.Args[0].Equals("Debug", StringComparison.CurrentCultureIgnoreCase))

            if (config != null && config.AssemblyElements.Any())
            {
                foreach (var assemblyElement in config.AssemblyElements)
                {
                    var assembly = Assembly.Load(assemblyElement.Name);

                    foreach (JobElement job in assemblyElement.Jobs)
                    {
                        var type = assembly.GetType(job.Class);

                        var injectedJob = JobBuilder.Create(type)
                            .WithIdentity(string.Format("{0}Job {1}", job.Name, Guid.NewGuid()), "Group")
                            .Build();

                        var injectedJobTrigger = TriggerBuilder.Create()
                            .WithIdentity(string.Format("{0}Trigger {1}", job.Name, Guid.NewGuid()), "Group")
                            .WithCronSchedule(job.CronTrigger) //0/5 * * * * ?
                            .Build();

                        scheduler.ScheduleJob(injectedJob, injectedJobTrigger);
                    }
                }
            }
        }

        public static void Stop()
        {
            scheduler.Shutdown();
            while (!scheduler.IsShutdown) { }
            scheduler = null;
        }
    }
}