using System;

namespace Testing.Jobs
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                if (input.Trim().ToLower().Equals("bet"))
                {
                    var job = new BET.Market.Jobs.ScrapeDailyRaces(); 
                    job.Run();
                }
                else if(input.Trim().ToLower().Equals("asx"))
                {
                    var job = new ASX.Market.Jobs.ScrapeDailyData();
                    job.Run();
                }
                
                Console.ReadKey();
            }
        }
    }
}