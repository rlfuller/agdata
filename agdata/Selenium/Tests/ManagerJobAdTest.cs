using agdata.Selenium.Pages;

namespace agdata.Selenium.Tests
{
    internal class ManagerJobAdTest : BaseTest
    {

        [Test]
        public void ClickManagerJobAdTest()
        {
            var homePage = new HomePage(driver);
            homePage.ClickMenuItem("Company", "Careers");

            var careersPage = new CareersPage(driver);
            careersPage.ClickOnSecondManagerJobListing();
        }
    }
}
