using Xunit;
using System;
using System.Linq;
using System.Threading;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.Common;
using GoogleMapsApi.Core.Entities.Places.Request;
using GoogleMapsApi.Core.Entities.Places.Response;
using GoogleMapsApi.Test.Fixtures;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class PlacesSearchTests
    {
        private readonly IntegrationTestFixture _fixture;

        public PlacesSearchTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ReturnsNearbySearchRequest()
        {
            var request = new PlacesRequest
            {
                ApiKey = _fixture.ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
            };

            PlacesResponse result = GoogleMaps.Places.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.True(result.Results.Count() > 5);
        }

        [Fact]
        public void TestNearbySearchType()
        {
            var request = new PlacesRequest
            {
                ApiKey = _fixture.ApiKey,
                Radius = 10000,
                Location = new Location(64.6247243, 21.0747553), // Skellefteå, Sweden
                Sensor = false,
                Type = "airport",
            };

            PlacesResponse result = GoogleMaps.Places.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.True(result.Results.Any());
            var correctAirport = result.Results.Where(t => t.Name.Contains("Skellefteå Airport"));
            Assert.True(correctAirport != null && correctAirport.Any());
        }

        [Fact]
        public void TestNearbySearchPagination()
        {
            var request = new PlacesRequest
            {
                ApiKey = _fixture.ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
            };

            PlacesResponse result = GoogleMaps.Places.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            //we should have more than one page of pizza results from the NearBy Search
            Assert.True(!String.IsNullOrEmpty(result.NextPage));
            //a full page of results is always 20
            Assert.True(result.Results.Count() == 20);
            var resultFromFirstPage = result.Results.FirstOrDefault(); //hold onto this

            //get the second page of results. Delay request by 2 seconds
            //Google API requires a short processing window to develop the second page. See Google API docs for more info on delay.
            
            Thread.Sleep(2000);
            request = new PlacesRequest
            {
                ApiKey = _fixture.ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
                PageToken = result.NextPage
            };
            result = GoogleMaps.Places.Query(request);
            Assert.Equal(Status.OK, result.Status);
            //make sure the second page has some results
            Assert.True(result.Results != null && result.Results.Any());
            //make sure the result from the first page isn't on the second page to confirm we actually got a second page with new results
            Assert.False(result.Results.Any(t => t.PlaceId == resultFromFirstPage.PlaceId));
        }
    }
}
