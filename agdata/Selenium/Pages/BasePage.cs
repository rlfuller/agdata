using agdata.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;

namespace agdata.Selenium.Pages
{
    internal abstract class BasePage
    {
        protected IWebDriver driver;
        protected ITestConfig config = TestConfigFactory.CurrentEnvironmentTestConfig;

        [FindsBy(How = How.Id, Using = "site-navigation")]
        private IWebElement navigationMenu { get; set; }


        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }


        /// <summary>
        /// Navigate to a page from the navigation menu at the top of the page
        /// </summary>
        /// <param name="mainMenuText">Top Level Menu item to hover over</param>
        /// <param name="subMenuText">Sub menu to click.</param>
        /// <returns></returns>
        public BasePage ClickMenuItem(string mainMenuText, string? subMenuText = null)
        {
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
            ClickMenuItem("Contact");
            return this;
        }

        private string constructXPathForMenu(string menuText)
        {
            return $"//a[contains(text(), '{menuText}')]";
        }
    }
}



