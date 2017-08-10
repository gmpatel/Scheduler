using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Scheduler.Service;
using Scheduler.Service.Interfaces;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceConfiguration.Configure();
        }        
    }
}