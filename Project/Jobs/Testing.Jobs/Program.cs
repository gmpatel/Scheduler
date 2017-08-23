using System;

namespace Testing.Jobs
{
    class Program
    {
        static void Main(string[] args)
        {
            var job = new ASX.Market.Jobs.ScrapeDailyData();
            //var job = new BET.Market.Jobs.ScrapeDailyRaces();
            job.Run();

            Console.ReadKey();
        }
    }
}