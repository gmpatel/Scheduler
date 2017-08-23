using System;
using System.Collections.Generic;
using System.Linq;
using BET.Market.Jobs.Core.Entities;
using BET.Market.Jobs.Core.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Quartz.Util;
using Scheduler.Core;

namespace BET.Market.Jobs
{
    public class ScrapeDailyRaces : SchedulerJobBase
    {
        private readonly string fileName;
        private readonly DateTime dateTime;

        public ScrapeDailyRaces()
        {
            this.dateTime = DateTime.Now;
            this.fileName = string.Format("{0}.{1:yyyyMMdd}.{2:HHmmss}", this.GetType().FullName, this.dateTime, this.dateTime);
        }

        public override void Run()
        {
            var exceptions = new List<Exception>();

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
                    var venues = new List<VenueEntity>();

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

                                if (raceNavigations.Any() && raceNavigations.Count == venues[i].Meetings.First().Races.Count + 2)
                                {
                                    foreach (var navigation in raceNavigations)
                                    {
                                        var href = navigation.FindElement(By.TagName("a")).GetAttribute("href");

                                        if (!string.IsNullOrEmpty(href))
                                        {
                                            navigation.FindElement(By.TagName("a")).Click();
                                            GeneralHelpers.Wait(3000);
                                        }
                                    }
                                }
                            }

                            for (int i = 0; i < venues.Count; i++)
                            {
                                driver.Navigate().GoToUrl(venues[i].Meetings.First().TipsUrl);
                                GeneralHelpers.Wait(3000);

                                var tipsRows = driver.GetElements(By.XPath("//*[@id='report']/tbody/tr"));

                                if (tipsRows != null && tipsRows.Any())
                                {
                                    venues[i].Meetings.First().Races = new List<RaceEntity>();

                                    foreach (var tip in tipsRows)
                                    {
                                        var r = GetRacesAndTips(tip);

                                        if (r != null)
                                        {
                                            venues[i].Meetings.First().Races.Add(r);
                                        }
                                    }
                                }
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

        private static VenueEntity GetMeetingData(IWebElement meeting)
        {
            var cells = meeting.FindElements(By.TagName("td"));

            if (cells != null && cells.Any())
            {
                var name = string.Empty;
                var province = string.Empty;
                
                var provinces = new List<string> {"ACT", "NSW", "VIC", "QLD", "SA", "TAS", "WA", "NT"};

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
                    return new VenueEntity { Name = name, Province = province, Meetings = new List<MeetingEntity> { new MeetingEntity { TipsUrl = tipsLink, FormUrl = formLink } } };
                }

                return null;
            }

            return null;
        }
    }
}