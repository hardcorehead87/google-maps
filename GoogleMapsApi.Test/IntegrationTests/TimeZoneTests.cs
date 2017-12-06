using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.Common;
using GoogleMapsApi.Core.Entities.TimeZone.Request;
using GoogleMapsApi.Core.Entities.TimeZone.Response;
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

            TimeZoneResponse result = GoogleMaps.TimeZone.QueryAsync(request).Result;

            Assert.Equal(Status.OK, result.Status);
        }
    }
}