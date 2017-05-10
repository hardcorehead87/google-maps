using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Test.Fixtures;
using Xunit;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class PlacesDetailsTests
    {
        private readonly IntegrationTestFixture _fixture;

        public PlacesDetailsTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        private string _cachedMyPlaceId;
        private string GetMyPlaceId()
        {
            if (_cachedMyPlaceId == null)
            {
                var request = new Entities.Places.Request.PlacesRequest()
                {
                    ApiKey = _fixture.ApiKey,
                    Name = "My Place Bar & Restaurant",
                    Location = new Location(-31.954453, 115.862717),
                    RankBy = Entities.Places.Request.RankBy.Distance,
                };
                var result = GoogleMaps.Places.Query(request);
                if (result.Status == Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                    Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
                _cachedMyPlaceId = result.Results.First().PlaceId;
            }
            return _cachedMyPlaceId;
        }

        [Fact]
        public void ReturnsPhotos()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = _fixture.ApiKey,
                PlaceId = "ChIJZ3VuVMQdLz4REP9PWpQ4SIY"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.NotEmpty(result.Result.Photos);
        }

        [Fact]
        public void ReturnsNotFoundForWrongReferenceString()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = _fixture.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                PlaceId = "ChIJbWWgrQAVkFQReAwrXXWzlYs"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.NOT_FOUND, result.Status);
        }

        readonly PriceLevel[] _anyPriceLevel = { PriceLevel.Free, PriceLevel.Inexpensive, PriceLevel.Moderate, PriceLevel.Expensive, PriceLevel.VeryExpensive };

        [Fact]
        public void ReturnsStronglyTypedPriceLevel()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = _fixture.ApiKey,
                PlaceId = GetMyPlaceId(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.Contains(result.Result.PriceLevel.Value, _anyPriceLevel);
        }

        [Fact]
        public void ReturnsOpeningTimes()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = _fixture.ApiKey,
                PlaceId = GetMyPlaceId(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            
            // commented out because seems like google doesn't have opening hours for this place anymore
            /*
            Assert.Equal(7, result.Result.OpeningHours.Periods.Count());
            var sundayPeriod = result.Result.OpeningHours.Periods.First();
            Assert.That(sundayPeriod.OpenTime.Day, Is.EqualTo(DayOfWeek.Sunday));
            Assert.That(sundayPeriod.OpenTime.Time, Is.GreaterThanOrEqualTo(0));
            Assert.That(sundayPeriod.OpenTime.Time, Is.LessThanOrEqualTo(2359));
            Assert.That(sundayPeriod.CloseTime.Time, Is.GreaterThanOrEqualTo(0));
            Assert.That(sundayPeriod.CloseTime.Time, Is.LessThanOrEqualTo(2359));
             */
        }
    }
}
