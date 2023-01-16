using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace agdata.Selenium.Pages
{
    internal class CareersPage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "li > .job > a")]
        private IList<IWebElement> jobs { get; set; }

        public CareersPage(IWebDriver driver): base(driver)
        {
        }

        public CareersPage MoveIntoIFrame()
        {
            //wait for iframe
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until<IWebDriver>(driver =>
            {
                return driver.SwitchTo().Frame(driver.FindElement(By.CssSelector("#openings #HBIFRAME")));
            });
            return this;
        }

        public CareersPage ReturnFromIFrame()
        {
            driver.SwitchTo().DefaultContent();
            return this;
        }

        public CareersPage AcceptCookies(int remaining = 1, bool ignoreException = true)
        {
            try
            {
                IWebElement dismissBtn = driver.FindElement(By.CssSelector(".cc-compliance > a.cc-dismiss"));
                dismissBtn.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(1)).Until<bool>(driver =>
                {
                    // Cookies popup takes some time to dissolve/animate.
                    return !dismissBtn.Displayed;
                });
            }
            catch (Exception e)
            {
                if (remaining > 0)
                {
                    // Sometimes it appears that the click event for the dismiss button isn't attached yet, so try again after a brief pause.
                    Thread.Sleep(200);
                    return AcceptCookies(remaining - 1);
                }
                if (!ignoreException)
                {
                    throw e;
                }
            }

            return this;
        }

        public CareersPage ClickOnSecondManagerJobListing()
        {
            MoveIntoIFrame();

            var elements = driver.FindElements(By.CssSelector("li > .job > a"));

            if (jobs.ToList().Count == 0)
            {
                throw new Exception("There are no job listings");
            }

            var managerJobs = jobs.Where(job => job.Text.Contains("Manager")).ToList();

            if (managerJobs.Count < 2)
            {
                throw new NoSuchElementException($"There are not 2 manager jobs. Count of Manager Jobs: {managerJobs.Count}");
            } 
            else
            {
                AcceptCookies();
                managerJobs[1].Click();
            }
            ReturnFromIFrame();
            return this;
        }
    }
}
