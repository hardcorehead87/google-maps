using System.Linq;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.Common;
using GoogleMapsApi.Core.Entities.Elevation.Request;
using GoogleMapsApi.Core.Entities.Elevation.Response;
using Xunit;

namespace GoogleMapsApi.Test.IntegrationTests
{
    public class ElevationTests
    {
        [Fact]
        public void Elevation_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

            var result = GoogleMaps.Elevation.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.Equal(14.78, result.Results.First().Elevation);
        }

        [Fact]
        public void ElevationAsync_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

            var result = GoogleMaps.Elevation.QueryAsync(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.Equal(14.78, result.Results.First().Elevation);
        } 
    }
}
