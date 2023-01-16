using agdata.Selenium.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agdata.Selenium.Tests
{
    internal class HelloWorld : BaseTest
    {

        [Test]
        public void HelloWorldTest()
        {
            Console.WriteLine("Rachel");

            // var search = new SearchResultsPage(driver);
            //  search.SearchForItem("pants");

            var basepage = new BasePage(driver);
            basepage.ClickMenuItem("Company", "Careers");
        }
    }
}
