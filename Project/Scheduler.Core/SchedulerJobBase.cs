using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Scheduler.Core.Common;
using Scheduler.Core.Exceptions;

namespace Scheduler.Core
{
    public abstract class SchedulerJobBase : ISchedulerJob
    {
        private readonly object locker = new object();

        public void Execute(IJobExecutionContext context)
        {
            var dateTime = DateTime.Now;
            var className = this.GetType().FullName;
            var text = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff tt} - {1}{2}Executing job as scheduled{3}{4}", dateTime, className, Environment.NewLine, Environment.NewLine, Environment.NewLine);
            var error = false;

            try
            {                
                Run();

                text = string.Format("{0}Succeed:{1}{2}", 
                    text, 
                    Environment.NewLine,
                    "Job completed successfully. There was no exception thrown to service by the job."
                );
            }
            catch (ExceptionBunch exceptions)
            {
                error = true;
                text = string.Format("{0}Failed:{1}{2}{3}{4}Error Message:{5}{6}{7}{8}Stack Trace:{9}{10}", 
                    text, 
                    Environment.NewLine, 
                    "Job completed with error(s). There was an exception thrown by the job to service.", 
                    Environment.NewLine, 
                    Environment.NewLine, 
                    Environment.NewLine, 
                    exceptions.AllMessages(), 
                    Environment.NewLine, 
                    Environment.NewLine, 
                    Environment.NewLine, 
                    exceptions.StackTrace
                );
            }
            finally
            {
                lock (locker)
                {
                    text = string.Format("{0}{1}{2}Finished:{3}{4}",
                        text,
                        Environment.NewLine,
                        Environment.NewLine,
                        Environment.NewLine,
                        string.Format("Job finished at {0:yyyy-MM-dd HH:mm:ss.fff tt}.", DateTime.Now)
                    );

                    File.WriteAllText(string.Format("{0}.log", className), text);
                    PrintConsoleMessage(className, text);

                    if (error)
                    {
                        File.WriteAllText(string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss.fff}.err", className, dateTime, dateTime), text);
                    }
                }
            }
        }

        public abstract void Run();

        private void PrintConsoleMessage(string className, string text)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 70));
            Console.WriteLine(className);
            Console.WriteLine(new string('-', 70));
            Console.Write(text); Console.WriteLine();
            Console.WriteLine(new string('-', 70));
            Console.WriteLine();
        }
    }
}