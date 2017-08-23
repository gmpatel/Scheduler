using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace BET.Market.Jobs.Core.Helpers
{
    public static class GeneralHelpers
    {
        public static void Wait(long miliseconds)
        {
            var startTime = DateTime.Now;

            while (DateTime.Now <= startTime.AddMilliseconds(miliseconds))
            {
                
            }
        }

        public static IWebElement GetElement(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IWebElement GetElement(this IWebElement element, By by)
        {
            try
            {
                return element.FindElement(by);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IReadOnlyCollection<IWebElement> GetElements(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElements(by);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IReadOnlyCollection<IWebElement> GetElements(this IWebElement element, By by)
        {
            try
            {
                return element.FindElements(by);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}