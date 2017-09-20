using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASX.Market.Jobs.Core.Entities;
using ASX.Market.Jobs.Core.Helpers;
using ASX.Market.Jobs.DataAccess.EF.Defaults;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Scheduler.Core;
using Scheduler.Core.Exceptions;

namespace ASX.Market.Jobs
{
    public class ScrapeStockPriceHistory : SchedulerJobBase
    {
        private readonly string fileName;
        private readonly DateTime dateTime;

        public IDataServiceASX DataService { get; private set; }

        public ScrapeStockPriceHistory() : this(new DataServiceASX())
        {
        }

        public ScrapeStockPriceHistory(IDataServiceASX dataService)
        {
            this.DataService = dataService;
            this.dateTime = DateTime.Now;
            this.fileName = string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss}", this.GetType().FullName, this.dateTime, this.dateTime);
        }

        public override void Run()
        {
            var exceptions = new List<Exception>();

            foreach (var e in this.DataService.GetStocks()) // GetStocksByCodes(new List<string> {"IIL", "BWX"})
            {
                Console.WriteLine("{0}, {1}", e.Code, e.Name);

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
                        driver.Navigate().GoToUrl(GetStockUrl(e));

                        var button = driver.GetElement(By.XPath("/html/body/div[2]/div[1]/div[4]/div[4]/div[2]/a[1]"));
                        button?.Click();

                        GeneralHelpers.Wait(5000);
                        var rows = driver.GetElements(By.XPath(@"//*[@id='rp_table']/tbody/tr"));

                        if (rows != null && rows.Any())
                        {
                            var list = new List<StockDetailEntity>();

                            foreach (var row in rows)
                            {
                                var cells = row.GetElements(By.TagName("td"));

                                if (cells.Any() && cells.Count >= 9)
                                {
                                    var detail = GetStockDetailEntity(cells);

                                    detail.StockId = e.Id;
                                    detail.Stock.Code = e.Code;

                                    list.Add(detail);
                                }
                            }

                            driver.Close();
                            
                            var resultedList = new List<StockDetailEntity>();

                            foreach (var s in list)
                            {
                                if (!DataService.CheckStockDetailExists(e.Id, s.Date))
                                {
                                    resultedList.Add(this.DataService.PushStockDetail(e.Id, s, this.dateTime));
                                }
                            }

                            File.WriteAllText(string.Format("{0}.{1}.json", fileName, e.Code), JsonConvert.SerializeObject(resultedList, Formatting.Indented));
                        }
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(new Exception(string.Format("Failed to scrape Stock {0} (id: {1}) from the url {2}", e.Code, e.Id, GetStockUrl(e)), exception));
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

        private static string GetStockUrl(StockEntity stock)
        {
            return string.Format("http://www.marketindex.com.au/asx/{0}", stock.Code.ToLower());
        }

        private StockDetailEntity GetStockDetailEntity(IReadOnlyCollection<IWebElement> cells)
        {
            var counter = 0;

            var stockDetailEntity = new StockDetailEntity
            {
                Stock = new StockEntity { }
            };

            foreach (var cell in cells)
            {
                switch (counter)
                {
                    case 0:
                    {
                        var date = long.Parse(DateTime.Parse(cell.Text).ToString("yyyyMMdd"));
                        stockDetailEntity.Date = date;
                        break;
                    }
                    case 1:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Price = val;
                        }

                        break;
                    }
                    case 2:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Change = val;
                        }

                        break;
                    }
                    case 3:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.ChangePercent = val;
                        }

                        break;
                    }
                    case 5:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.High = val;
                        }

                        break;
                    }
                    case 6:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Low = val;
                        }

                        break;
                    }
                    case 7:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.Volume = val;
                        }

                        break;
                    }
                    case 8:
                    {
                        decimal val;

                        if (decimal.TryParse(FilterNumber(cell.Text), out val))
                        {
                            stockDetailEntity.MarketCapital = val;
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