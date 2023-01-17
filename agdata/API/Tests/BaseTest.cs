using agdata.Configuration;
using RestSharp;

namespace agdata.API.Tests
{
    /// <summary>
    /// Superclass for API Tests. Creates rest client and configuration.
    /// </summary>
    internal class BaseTest
    {
        protected RestClient client;
        protected ITestConfig config = TestConfigFactory.CurrentEnvironmentTestConfig;

        [SetUp]
        public void Setup()
        {
            client = new RestClient(config.ApiBaseUrl);
        }
    }
}
