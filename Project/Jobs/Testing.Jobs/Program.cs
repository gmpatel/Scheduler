using System;

namespace Testing.Jobs
{
    class Program
    {
        static void Main(string[] args)
        {
            var job = new ASX.Market.Jobs.ScrapeDailyData();
            job.Run();

            Console.ReadKey();
        }
    }
}