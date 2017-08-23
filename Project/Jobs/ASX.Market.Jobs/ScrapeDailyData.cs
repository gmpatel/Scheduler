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
using ASX.Market.Jobs.Core.Helpers;
using ASX.Market.Jobs.DataAccess.EF.Defaults;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Scheduler.Core;
using Scheduler.Core.Common;
using Scheduler.Core.Exceptions;

namespace ASX.Market.Jobs
{
    public class ScrapeDailyData : SchedulerJobBase
    {
        private readonly string fileName;
        private readonly DateTime dateTime;

        public IDataService DataService { get; private set; }

        public ScrapeDailyData() : this(new DataService())
        {
        }

        public ScrapeDailyData(IDataService dataService)
        {
            this.DataService = dataService;
            this.dateTime = DateTime.Now;
            this.fileName = string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss}", this.GetType().FullName, this.dateTime, this.dateTime);
        }

        public override void Run()
        {
            var exceptions = new List<Exception>();

            foreach (var e in this.DataService.GetIndices().OrderByDescending(x => x.Id)) // GetIndicesByCodes(new List<string> { "AllOrds" })
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
                        GeneralHelpers.Wait(3000);

                        button?.Click();

                        GeneralHelpers.Wait(10000);
                        var rows = driver.GetElements(By.XPath(@"//*[@id='asx_sp_table']/tbody/tr"));

                        if (rows != null && rows.Any())
                        {
                            var list = new List<StockDetailEntity>();

                            foreach (var row in rows)
                            {
                                var cells = row.GetElements(By.TagName("td"));

                                if (cells.Any() && cells.Count >= 11)
                                {
                                    list.Add(GetStockDetailEntity(cells));
                                }
                            }

                            driver.Close();

                            var resultedList = new List<StockDetailEntity>();

                            foreach (var s in list)
                            {
                                resultedList.Add(this.DataService.PushStockDetail(e.Id, s, this.dateTime));
                            }

                            File.WriteAllText(string.Format("{0}.{1}.json", fileName, e.Code), JsonConvert.SerializeObject(resultedList, Formatting.Indented));
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

                        try
                        {
                            driver.Dispose();
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
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                        {
                            stockDetailEntity.Date = long.Parse(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                        }
                        else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                        {
                            stockDetailEntity.Date = long.Parse(DateTime.Now.AddDays(-2).ToString("yyyyMMdd"));
                        }
                        else if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                        {
                            var currTime = long.Parse(DateTime.Now.ToString("HHmmss"));
                            stockDetailEntity.Date = currTime < 100000 ? long.Parse(DateTime.Now.AddDays(-3).ToString("yyyyMMdd")) : long.Parse(DateTime.Now.ToString("yyyyMMdd"));
                        }
                        else
                        {
                            var currTime = long.Parse(DateTime.Now.ToString("HHmmss"));
                            stockDetailEntity.Date = currTime < 100000 ? long.Parse(DateTime.Now.AddDays(-1).ToString("yyyyMMdd")) : long.Parse(DateTime.Now.ToString("yyyyMMdd"));
                        }
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