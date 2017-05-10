using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using Xunit;

namespace GoogleMapsApi.Test.IntegrationTests
{
    public class TimeZoneTests
    {
        [Fact]
        public void TimeZone_Correct_OverviewPath()
        {
            TimeZoneRequest request = new TimeZoneRequest();
            request.Location = new Location(55.866413, 12.501063);
            request.Language = "en";

            TimeZoneResponse result = GoogleMaps.TimeZone.Query(request);

            Assert.Equal(Status.OK, result.Status);
        }
    }
}