using agdata.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agdata.Selenium.Pages
{
    internal class BasePage
    {
        protected IWebDriver driver;
        protected ITestConfig config = TestConfigFactory.CurrentEnvironmentTestConfig;

        [FindsBy(How = How.Id, Using = "site-navigation")]
        private IWebElement navigationMenu { get; set; }

        //private IWebElement companyMenu = navigationMenu.FindElement(By.)

        // [FindsBy(How = How.XPath, Using =".//#site-navigation/ ")]
        // private IWebElement companyMenu { get;set }


        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
           // WaitUntilReady();
            PageFactory.InitElements(driver, this);
        }


        public BasePage ClickMenuItem(string mainMenuText, string? subMenuText = null)
        {
            // IWebElement ele = navigationMenu.FindElement(By.XPath($"//a[contains(text(), '{hoverText}')]"));

            // var element = wait.Until(ExpectedConditions.El)
            string xPath = constructXPathForMenu(mainMenuText);
            IWebElement ele = navigationMenu.FindElement(By.XPath(xPath));
            Actions action = new Actions(driver);
            action.MoveToElement(ele);

            if (subMenuText != null)
            {
                //get the submenu
                xPath = constructXPathForMenu(subMenuText);
                IWebElement subMenu = navigationMenu.FindElement(By.XPath(xPath));
                action.MoveToElement(subMenu);
            }

            action.Click().Build().Perform();
            return this;
        }

        public BasePage ClickSubMenu(string menuText)
        {
           // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            ClickMenuItem("Contact");
            return this;
        }

        private string constructXPathForMenu(string menuText)
        {
            return $"//a[contains(text(), '{menuText}')]";
        }
    }
}



