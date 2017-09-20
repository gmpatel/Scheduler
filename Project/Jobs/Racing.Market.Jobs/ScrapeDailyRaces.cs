using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using BET.Market.Jobs.Core.Entities;
using BET.Market.Jobs.Core.Helpers;
using BET.Market.Jobs.DataAccess.EF.Defaults;
using BET.Market.Jobs.DataAccess.EF.Interfaces;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Quartz.Util;
using Scheduler.Core;
using Scheduler.Core.Exceptions;

namespace BET.Market.Jobs
{
    public class ScrapeDailyRaces : SchedulerJobBase
    {
        private readonly string fileName;
        private readonly DateTime dateTime;

        public IDataServiceBET DataService { get; private set; }

        public ScrapeDailyRaces() : this(new DataServiceBET())
        {
        }

        public ScrapeDailyRaces(IDataServiceBET dataService)
        {
            this.DataService = dataService;
            this.dateTime = DateTime.Now;
            this.fileName = string.Format("{0}.{1:yyyyMMdd}.{2:HH}0000", this.GetType().FullName, this.dateTime, this.dateTime);
        }

        public override void Run()
        {
            IList<VenueEntity> data;

            if (File.Exists(string.Format("{0}.json", fileName)))
            {
                data = JsonConvert.DeserializeObject<IList<VenueEntity>>(File.ReadAllText(string.Format("{0}.json", fileName)));
            }
            else
            {
                data = GetRaces(); 
            }

            DataService.UpdateData(data);
        }

        public IList<VenueEntity> GetRaces()
        {
            var exceptions = new List<Exception>();
            var venues = new List<VenueEntity>();

            Console.WriteLine("");

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
                    driver.Navigate().GoToUrl("https://www.skyracing.com.au/tab/form/index.php#");
                    GeneralHelpers.Wait(3000);

                    var headElement = driver.GetElement(By.XPath("//*[@id='headname']"));

                    if (headElement != null)
                    {
                        var meetingRows = driver.GetElements(By.XPath("//*[@id='wrapper']/table[1]/tbody/tr"));

                        if (meetingRows != null && meetingRows.Any())
                        {
                            foreach (var meeting in meetingRows)
                            {
                                var m = GetMeetingData(meeting);

                                if (m != null)
                                {
                                    venues.Add(m);
                                }
                            }
                        }

                        if (venues.Any())
                        {
                            for (int i = 0; i < venues.Count; i++)
                            {
                                driver.Navigate().GoToUrl(venues[i].Meetings.First().FormUrl);
                                GeneralHelpers.Wait(3000);

                                var raceNavigations = driver.GetElements(By.XPath("//*[@id='navlist']/li"));

                                if (raceNavigations.Any())
                                {
                                    var raceCounter = 0;

                                    for (var x = 0; x < raceNavigations.Count; x++)
                                    {
                                        raceNavigations = driver.GetElements(By.XPath("//*[@id='navlist']/li"));

                                        var href = raceNavigations.ElementAt(x).FindElement(By.TagName("a")).GetAttribute("href");

                                        if (!string.IsNullOrEmpty(href))
                                        {
                                            raceCounter++;

                                            raceNavigations.ElementAt(x).FindElement(By.TagName("a")).Click();
                                            GeneralHelpers.Wait(3000);

                                            var race = new RaceEntity
                                            {
                                                Number = raceCounter,
                                                Name = driver.GetElement(By.XPath("//*[@id='topleft']/table[1]/tbody/tr/td[1]"))?.Text.Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Time = driver.GetElement(By.XPath("//*[@id='topleft']/table[1]/tbody/tr/td[2]"))?.Text.Replace("Advertised Start Time:", "").Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Weather = driver.GetElement(By.XPath("//*[@id='topleft']/table[1]/tbody/tr/td[3]"))?.Text.Replace("Weather:", "").Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Track = driver.GetElement(By.XPath("//*[@id='topleft']/table[1]/tbody/tr/td[4]"))?.Text.Replace("Track:", "").Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Distance = driver.GetElement(By.XPath("//*[@id='topleft']/table[2]/tbody/tr/td[1]"))?.Text.Replace("Distance:", "").Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Class = driver.GetElement(By.XPath("//*[@id='topleft']/table[2]/tbody/tr/td[2]"))?.Text.Replace("Class:", "").Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Prizemoney = driver.GetElement(By.XPath("//*[@id='topleft']/table[2]/tbody/tr/td[3]"))?.Text.Replace("Prizemoney:", "").Replace("\r\n", "").Replace("\n", "").Trim(),
                                                Runners = new List<RunnerEntity>(),
                                                DateTimeCreated = dateTime
                                            };
                                            
                                            var table = driver.GetElement(By.XPath("//*[@id='report']/tbody"));
                                            var runners = table.FindElements(By.XPath("//*[@class='alt' or @class='row' or @class='altscratched' or @class='rowscratched']"));

                                            if (runners.Any())
                                            {
                                                foreach (var runner in runners)
                                                {
                                                    var scratched = runner.GetAttribute("class").Contains("scratched");
                                                    var cells = runner.FindElements(By.TagName("td"));

                                                    if (cells.Any())
                                                    {
                                                        var counter = 0;
                                                        var r = new RunnerEntity { Scratched = scratched, DateTimeCreated = dateTime };

                                                        foreach (var cell in cells)
                                                        {
                                                            counter++;

                                                            if (counter == 1)
                                                            {
                                                                r.Number = int.Parse(cell.Text.Split(new char [] {'\n'}).Last().Replace("a", "").Replace("b", "").Replace("c", "").Replace("d", "").Replace("e", "").Replace("f", "").Replace("g", "").Replace("h", "").Trim());
                                                            }
                                                            else if (counter == 2)
                                                            {
                                                                r.LastFiveRuns = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();
                                                            }
                                                            else if (counter == 3)
                                                            {
                                                                r.Name = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();
                                                            }
                                                            else if (counter == 4)
                                                            {
                                                                r.Barrel = int.Parse(cell.Text.Replace("\r\n", "").Replace("\n", "").Trim());
                                                            }
                                                            else if (counter == 5)
                                                            {
                                                                r.Tcdw = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();
                                                            }
                                                            else if (counter == 6)
                                                            {
                                                                r.Trainer = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();
                                                            }
                                                            else if (counter == 7)
                                                            {
                                                                r.Jockey = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();
                                                            }
                                                            else if (counter == 8)
                                                            {
                                                                r.Weight = decimal.Parse(cell.Text.Replace("\r\n", "").Replace("\n", "").Trim());
                                                            }
                                                            else if (counter == 9)
                                                            {
                                                                r.Rating = int.Parse(cell.Text.Replace("\r\n", "").Replace("\n", "").Trim());
                                                            }
                                                        }

                                                        race.Runners.Add(r);
                                                    }
                                                }
                                            }

                                            var formRows = driver.GetElements(By.XPath("//*[@id='details']/tbody/tr"));

                                            if (formRows.Any())
                                            {
                                                var rowCounter = 0;

                                                foreach (var formRow in formRows)
                                                {
                                                    var cells = formRow.GetElements(By.TagName("td"));

                                                    if (cells.Any())
                                                    {
                                                        var cellCounter = 0;
                                                        rowCounter++;

                                                        foreach (var cell in cells)
                                                        {
                                                            cellCounter++;

                                                            var text = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();

                                                            if (!string.IsNullOrEmpty(text))
                                                            {
                                                                var number = int.Parse(text.Split(new char[] { ' ' })[0].Trim());
                                                                var run = race.Runners.FirstOrDefault(xr => xr.Number == number);

                                                                if (rowCounter <= 4)
                                                                {
                                                                    if (run != null)
                                                                    {
                                                                        if (cellCounter == 1)
                                                                        {
                                                                            run.FormSkyRating = true;
                                                                            run.FormSkyRatingPosition = rowCounter;
                                                                        }
                                                                        else if (cellCounter == 2)
                                                                        {
                                                                            run.FormBest12Months = true;
                                                                            run.FormBest12MonthsPosition = rowCounter;
                                                                        }
                                                                        else if (cellCounter == 3)
                                                                        {
                                                                            run.FormRecent = true;
                                                                            run.FormRecentPosition = rowCounter;
                                                                        }
                                                                        else if (cellCounter == 4)
                                                                        {
                                                                            run.FormDistance = true;
                                                                            run.FormDistancePosition = rowCounter;
                                                                        }
                                                                    }
                                                                }
                                                                else if (rowCounter > 4)
                                                                {
                                                                    if (run != null)
                                                                    {
                                                                        if (cellCounter == 1)
                                                                        {
                                                                            run.FormClass = true;
                                                                            run.FormClassPosition = rowCounter - 4;
                                                                        }
                                                                        else if (cellCounter == 2)
                                                                        {
                                                                            run.FormTimeRating = true;
                                                                            run.FormTimeRatingPosition = rowCounter - 4;
                                                                        }
                                                                        else if (cellCounter == 3)
                                                                        {
                                                                            run.FormInWet = true;
                                                                            run.FormInWetPosition = rowCounter - 4;
                                                                        }
                                                                        else if (cellCounter == 4)
                                                                        {
                                                                            run.FormBestOverall = true;
                                                                            run.FormBestOverallPosition = rowCounter - 4;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            venues[i].Meetings.First().Races.Add(race);
                                        }
                                    }
                                }

                                if (venues[i].Meetings?.First()?.Races?.Count > 0)
                                {
                                    driver.Navigate().GoToUrl(venues[i].Meetings.First().TipsUrl);
                                    GeneralHelpers.Wait(3000);

                                    var tipsRows = driver.GetElements(By.XPath("//*[@id='report']/tbody/tr"));
                                    
                                    if (tipsRows != null && tipsRows.Any())
                                    {
                                        var raceCounter = 0;

                                        foreach (var tip in tipsRows)
                                        {
                                            var cells = tip.FindElements(By.TagName("td"));

                                            if (cells.Any())
                                            {
                                                var cellCounter = 0;
                                                raceCounter++;

                                                foreach (var cell in cells)
                                                {
                                                    cellCounter++;
                                                    var text = cell.Text.Replace("\r\n", "").Replace("\n", "").Trim();

                                                    if (!string.IsNullOrEmpty(text) && cellCounter >= 3 && cellCounter <= 6)
                                                    {
                                                        var race = venues[i].Meetings.First().Races.FirstOrDefault(xr => xr.Number == raceCounter);
                                                        var number = 0;

                                                        if(race != null && int.TryParse(text.Split(new char[] { ' ' })[0].Replace(".", "").Trim(), out number))
                                                        {
                                                            var run = race.Runners.FirstOrDefault(xr => xr.Number == number);
                                                            if (run != null)
                                                            {
                                                                run.TipSky = true;
                                                                run.TipSkyPosition = cellCounter - 2;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                File.WriteAllText(string.Format("{0}.json", fileName), JsonConvert.SerializeObject(venues, Formatting.Indented));
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    exceptions.Add(new Exception(string.Format("Failed to scrape Index {0} (id: {1}, exchangeId: {2} (exchange: {3})) from the url {4}", "", "", "", "", ""), exception));
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

            if (exceptions.Any())
            {
                throw new ExceptionBunch("There were bunch of exceptions during the scrapping process.", exceptions); {}
            }

            return venues;
        }
        
        private static RaceEntity GetRacesAndTips(IWebElement tip)
        {
            var cells = tip.FindElements(By.TagName("td"));

            if (cells != null && cells.Any())
            {
                var counter = 0;
                var number = 0;
                var name = string.Empty;

                var race = default(RaceEntity);

                foreach (var cell in cells)
                {
                    var text = cell.Text.Trim();
                    
                    if (counter == 0)
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            number = int.Parse(text);
                        }
                    }
                    else if (counter == 1)
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            name = text;
                        }

                        if (number > 0 && !string.IsNullOrEmpty(name))
                        {
                            race = new RaceEntity
                            {
                                Number = number,
                                Name = name,
                                Runners = new List<RunnerEntity>()
                            };
                        }
                    }

                    if (counter >= 2 && counter <= 5 && race != null)
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            var parts = text.Split(new char[] { '.' });

                            if (parts.Length == 2)
                            {
                                var tipNumber = int.Parse(parts[0].Trim());
                                var tipName = parts[1].Trim();

                                race.Runners.Add(new RunnerEntity
                                {
                                    Number = tipNumber,
                                    Name = tipName,
                                    TipSky = true
                                });
                            }
                        }
                    }

                    counter++;
                }
                
                return race;
            }

            return null;
        }

        private VenueEntity GetMeetingData(IWebElement meeting)
        {
            var cells = meeting.FindElements(By.TagName("td"));

            if (cells != null && cells.Any())
            {
                var name = string.Empty;
                var province = string.Empty;
                
                var provinces = new List<string> {"ACT", "NSW", "VIC", "QLD", "SA", "TAS", "WA", "NT", "UK", "FR", "IR"};

                var nameState = cells[0].Text;

                var parts = nameState.Split(new char[] { '(' });

                if (parts.Length > 1)
                {
                    name = parts[0].Trim().ToUpper();
                    province = parts[1].Replace("(", "").Replace(")", "").Trim().ToUpper();
                }

                var tipsLink = cells[1].FindElement(By.TagName("a")).GetAttribute("href"); //string.Format("{0}{1}", baseUrl, );
                var formLink = cells[2].FindElement(By.TagName("a")).GetAttribute("href"); //string.Format("{0}{1}", baseUrl, );

                if (provinces.Contains(province))
                {
                    return new VenueEntity { Name = name, Province = province, Meetings = new List<MeetingEntity> { new MeetingEntity { Date = long.Parse(DateTime.Now.ToString("yyyyMMdd")), TipsUrl = tipsLink, FormUrl = formLink, Races = new List<RaceEntity>(), DateTimeCreated = dateTime } }, DateTimeCreated = dateTime };
                }

                return null;
            }

            return null;
        }
    }
}