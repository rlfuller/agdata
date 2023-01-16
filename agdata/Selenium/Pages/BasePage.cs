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



