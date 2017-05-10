using Microsoft.Extensions.Configuration;
using Xunit;

namespace GoogleMapsApi.Test.Fixtures
{
    //  Note:	The integration tests run against the real Google API web
    //			servers and count towards your query limit. Also, the tests
    //			require a working internet connection in order to pass.
    //			Their run time may vary depending on your connection,
    //			network congestion and the current load on Google's servers.
    public class IntegrationTestFixture
    {
        private string _apiKey;
        public string ApiKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_apiKey))
                    Assert.True(false, "API key not specified, please set it in the 'appsettings.json' file");
                return _apiKey;
            }
            private set => _apiKey = value;
        }
        
        public IntegrationTestFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            ApiKey = config["ApiKey"];
        }
    }

    [CollectionDefinition("IntegrationTest")]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {
    }
}
