using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service.Interfaces
{
    public interface ISchedulerService
    {
        void Start();
        void Stop();
    }
}