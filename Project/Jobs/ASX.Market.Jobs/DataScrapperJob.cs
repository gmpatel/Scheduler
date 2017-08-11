using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ASX.Market.Jobs.DataAccess.EF.Defaults;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;
using OpenQA.Selenium.Chrome;
using Scheduler.Core;

namespace ASX.Market.Jobs
{
    public class DataScrapperJob : SchedulerJobBase
    {
        public IDataService DataService { get; private set; }

        public DataScrapperJob() : this(new DataService())
        {
        }

        public DataScrapperJob(IDataService dataService)
        {
            this.DataService = dataService;
        }

        public override void Run()
        {
            foreach (var e in this.DataService.GetIndices())
            {
                Console.WriteLine(e.Name);
            }

            var chromeService = ChromeDriverService.CreateDefaultService();
            chromeService.HideCommandPromptWindow = true;

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("-incognito");
            chromeOptions.AddArguments("--disable-popup-blocking");


            using (var driver = new ChromeDriver(chromeService, chromeOptions))
            {
                try
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                    driver.Navigate().GoToUrl("http://www.marketindex.com.au/asx200");
                    var fileName = string.Format("ASX.Market.Jobs.DataScrapperJob.{0:yyyyMMdd}.{1:HHmmss}.data", DateTime.Now, DateTime.Now);
                    var text = "http://www.marketindex.com.au/asx200 was navigated successfully...";
                    var element = driver.FindElementById("sub_footer");
                    if (element != null)
                    {
                        text = element.Text;
                    }
                    File.WriteAllText(fileName, text);
                }
                catch (Exception)
                {
                    // Do ...
                }
                finally
                {
                    try
                    {
                        //driver.Close();
                    }
                    catch (Exception exception)
                    {
                        var fileName = string.Format("ASX.Market.Jobs.DataScrapperJob.{0:yyyyMMdd}.{1:HHmmss}.err", DateTime.Now, DateTime.Now);
                        File.WriteAllText(fileName, string.Format("{0}\\n\\n{1}",exception.Message, exception.StackTrace));
                    }
                }
            }
        }
    }
}