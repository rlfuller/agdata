namespace agdata.Configuration
{
    /// <summary>
    /// Used to store environment variables needed to run tests in QA enviornment
    /// </summary>
    internal class QATestConfig : ITestConfig
    {
        string ITestConfig.BaseUrl => "https://www.agdata.com/";

        string ITestConfig.Username => "QAUserName";

        string ITestConfig.Password => "QAPassword";

        string ITestConfig.UserFirstName => "QAUserFirstName";

        string ITestConfig.UserLastName => "QAUserLastName";

        string ITestConfig.ApiBaseUrl => "https://jsonplaceholder.typicode.com/";
    }
}
