using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //var job = new ASX.Market.Jobs.DataScrapperJob();
            //job.Run();

            string fullyQualifiedName = typeof(ASX.Market.Jobs.DataScrapperJob).FullName;

            Console.ReadKey();
        }
    }
}
