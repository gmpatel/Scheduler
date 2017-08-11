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
            var job = new ASX.Market.Jobs.DataScrapperJob();
            job.Run();

            Console.ReadKey();
        }
    }
}