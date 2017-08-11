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

            var id = Guid.NewGuid();

            var builtInJob = JobBuilder.Create<ServiceTickerJob>()
                .WithIdentity(string.Format("Scheduler.Jobs.Defaults.ServiceTickerJob.J.{0}", id), "Group")
                .Build();

            var builtInJobTrigger = TriggerBuilder.Create()
                .WithIdentity(string.Format("Scheduler.Jobs.Defaults.ServiceTickerJob.T.{0}", id), "Group")
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
                        var name = type.FullName;
                        var guid = Guid.NewGuid();

                        var injectedJob = JobBuilder.Create(type)
                            .WithIdentity(string.Format("{0}.J.{1}", name, guid), "Group")
                            .Build();

                        var injectedJobTrigger = TriggerBuilder.Create()
                            .WithIdentity(string.Format("{0}.T.{1}", name, guid), "Group")
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