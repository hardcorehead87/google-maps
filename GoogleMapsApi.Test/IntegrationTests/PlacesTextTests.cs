using System.Linq;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.PlacesText.Request;
using GoogleMapsApi.Core.Entities.PlacesText.Response;
using GoogleMapsApi.Test.Fixtures;
using Xunit;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class PlacesTextTests
    {
        private readonly IntegrationTestFixture _fixture;

        public PlacesTextTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ReturnsFormattedAddress()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = _fixture.ApiKey,
                Query = "1 smith st parramatta",
                Types = "address"
            };

            PlacesTextResponse result = GoogleMaps.PlacesText.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.Equal("1 Smith St, Parramatta NSW 2150, Australia", result.Results.First().FormattedAddress);
        }

        [Fact]
        public void ReturnsPhotos()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = _fixture.ApiKey,
                Query = "1600 Pennsylvania Ave NW",
                Types = "address"
            };

            PlacesTextResponse result = GoogleMaps.PlacesText.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.NotNull(result.Results);
            Assert.NotEmpty(result.Results);
        }
    }
}
