using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.Core.Exceptions;
using ASX.Market.Jobs.Core.Helpers;
using ASX.Market.Jobs.DataAccess.EF.Defaults;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Scheduler.Core;
using Scheduler.Core.Common;

namespace ASX.Market.Jobs
{
    public class DataScrapperJob : SchedulerJobBase
    {
        private readonly string fileName;
        private readonly string errorFileName;

        public IDataService DataService { get; private set; }

        public DataScrapperJob() : this(new DataService())
        {
        }

        public DataScrapperJob(IDataService dataService)
        {
            this.DataService = dataService;
            this.fileName = string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss}.json", this.GetType().FullName, DateTime.Now, DateTime.Now);
            this.errorFileName = string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss}.err", this.GetType().FullName, DateTime.Now, DateTime.Now);
        }

        public override void Run()
        {
            var exceptions = new List<Exception>();

            foreach (var e in this.DataService.GetIndices())
            {
                Console.WriteLine(e.Name);

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
                        driver.Navigate().GoToUrl(e.Url);

                        var button = driver.GetElement(By.XPath("/html/body/div[2]/div[9]/div/div[3]/div/a[1]"));
                        button?.Click();

                        GeneralHelpers.Wait(5000);
                        var rows = driver.GetElements(By.XPath(@"//*[@id='asx_sp_table']/tbody/tr"));

                        if (rows != null && rows.Any())
                        {
                            foreach (var row in rows)
                            {
                                var cells = row.GetElements(By.TagName("td"));

                                if (cells.Any() && cells.Count >= 11)
                                {
                                    var stockDetailEntity = GetStockDetailEntity(cells);
                                    stockDetailEntity = this.DataService.PushStockDetail(e.Id, stockDetailEntity);
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(new Exception(string.Format("Failed to scrape Index {0} (id: {1}, exchangeId: {2} (exchange: {3})) from the url {4}", e.Code, e.Id, e.ExchangeId, e.Exchange.Name, e.Url), exception));
                    }
                    finally
                    {
                        try
                        {
                            driver.Close();
                        }
                        catch (Exception)
                        {
                            // Do ...
                        }
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new ExceptionBunch(string.Format("There are {0} processes of the job have been failed.", exceptions.Count), exceptions);
            }
        }

        private StockDetailEntity GetStockDetailEntity(IReadOnlyCollection<IWebElement> cells)
        {
            var counter = 0;

            var stockDetailEntity = new StockDetailEntity
            {
                Stock = new StockEntity {}
            };

            foreach (var cell in cells)
            {
                switch (counter)
                {
                    case 0:
                    {
                        stockDetailEntity.Date = long.Parse(DateTime.Now.ToString("yyyyMMdd"));
                        break;
                    }
                    case 1:
                    {
                        var flag = false;
                        stockDetailEntity.Stock.Code = FilterText(cell.Text, out flag);
                        break;
                    }
                    case 2:
                    {
                        var flag = false;
                        stockDetailEntity.Stock.Name = FilterText(cell.Text, out flag);
                        stockDetailEntity.Flag1 = flag;
                        break;
                    }
                    case 3:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Price = val;
                        }

                        break;
                    }
                    case 4:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Change = val;
                        }

                        break;
                    }
                    case 5:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.ChangePercent = val;
                        }

                        break;
                    }
                    case 6:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.High = val;
                        }

                        break;
                    }
                    case 7:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Low = val;
                        }

                        break;
                    }
                    case 8:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Volume = val;
                        }

                        break;
                    }
                    case 9:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.MarketCapital = val;
                        }

                        break;
                    }
                    case 10:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.OneYearChange = val;
                        }
                            
                        break;
                    }
                }

                counter++;
            }
            
            return stockDetailEntity;
        }

        private static string FilterText(string text, out bool flag)
        {
            var ret = text
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim();

            flag = false || text.Contains("Strong Buy");
            ret = ret.Replace("Strong Buy", "").Trim();

            return ret;
        }

        private static string FilterNumber(string text)
        {
            return text
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("$", "")
                .Replace(",", "")
                .Replace("%", "")
                .Replace(" ", "")
                .Trim();
        }
    }
}