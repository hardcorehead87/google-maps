using Xunit;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.Common;
using GoogleMapsApi.Core.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Core.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Test.Fixtures;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class PlaceAutocompleteTests
    {
        private readonly IntegrationTestFixture _fixture;

        public PlaceAutocompleteTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ReturnsNoResults()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = _fixture.ApiKey,
                Input = "zxqtrb",
                Location = new Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.ZERO_RESULTS, result.Status);
        }

        [Fact]
        public void OffsetTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = _fixture.ApiKey,
                Input = "abbeyjibberish",
                Location = new Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.ZERO_RESULTS, result.Status);

            var offsetRequest = new PlaceAutocompleteRequest
            {
                ApiKey = _fixture.ApiKey,
                Input = "abbeyjibberish",
                Offset = 5,
                Location = new Location(53.4635332, -2.2419169)
            };

            PlaceAutocompleteResponse offsetResult = GoogleMaps.PlaceAutocomplete.Query(offsetRequest);

            if (offsetResult.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, offsetResult.Status);
        }

        [Fact]
        public void TypeTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = _fixture.ApiKey,
                Input = "abb",
                Type = "geocode",
                Location = new Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");

            Assert.Equal(Status.OK, result.Status);

            foreach (var oneResult in result.Results)
            {
                Assert.NotNull(oneResult.Types);
                Assert.True(new List<string>(oneResult.Types).Contains("geocode"), "non-geocode result");
            }
        }
        
        [Fact]
        public void CheckForExpectedRoad1()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = _fixture.ApiKey,
                Input = "oakfield road, chea",
                Location = new Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.NotEqual(Status.ZERO_RESULTS, result.Status);

            Assert.True(result.Results.Any(t => t.Description.ToUpper().Contains("CHEADLE")));
        }

        [Fact]
        public void CheckForExpectedRoad2()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = _fixture.ApiKey,
                Input = "128 abbey r",
                Location = new Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.NotEqual(Status.ZERO_RESULTS, result.Status);

            Assert.True(result.Results.Any(t => t.Description.ToUpper().Contains("MACCLESFIELD")));
        }

        [Fact]
        public void CheckZeroRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 0);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertRequestsLimit(result);
            Assert.NotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Fact]
        public void CheckNegativeRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", -1);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertRequestsLimit(result);
            Assert.NotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Fact]
        public void CheckLargerThenEarthRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 30000000);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertRequestsLimit(result);
            Assert.NotEqual(Status.ZERO_RESULTS, result.Status);
        }

        private void AssertRequestsLimit(PlaceAutocompleteResponse result) 
        {
            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
        }

        private PlaceAutocompleteRequest CreatePlaceAutocompleteRequest(string query, double? radius) 
        {
            return new PlaceAutocompleteRequest 
            {
                ApiKey = _fixture.ApiKey,
                Input = query,
                Location = new Location(0, 0),
                Radius = radius
            };
        }
    }
}
