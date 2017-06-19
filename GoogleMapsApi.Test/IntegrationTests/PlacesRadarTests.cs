using Xunit;
using System.Linq;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.Common;
using GoogleMapsApi.Core.Entities.PlacesRadar.Request;
using GoogleMapsApi.Core.Entities.PlacesRadar.Response;
using GoogleMapsApi.Test.Fixtures;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class PlacesRadarTests
    {
        private readonly IntegrationTestFixture _fixture;

        public PlacesRadarTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ReturnsRadarSearchRequest()
        {
            var request = new PlacesRadarRequest
            {
                ApiKey = _fixture.ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
            };

            PlacesRadarResponse result = GoogleMaps.PlacesRadar.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.True(result.Results.Count() > 5);
        }

        [Fact]
        public void TestRadarSearchType()
        {
            var request = new PlacesRadarRequest
            {
                ApiKey = _fixture.ApiKey,
                Radius = 10000,
                Location = new Location(64.6247243, 21.0747553), // Skellefteå, Sweden
                Sensor = false,
                Type = "airport",
            };

            PlacesRadarResponse result = GoogleMaps.PlacesRadar.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.True(result.Results.Any());
        }
    }
}