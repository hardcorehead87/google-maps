using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
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

            if (result.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.Equal(14.78, result.Results.First().Elevation);
        }

        [Fact]
        public void ElevationAsync_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

            var result = GoogleMaps.Elevation.QueryAsync(request).Result;

            if (result.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.Equal(14.78, result.Results.First().Elevation);
        } 
    }
}
