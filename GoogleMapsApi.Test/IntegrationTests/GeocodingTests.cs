using System;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Entities.Common;
using GoogleMapsApi.Core.Entities.Geocoding.Request;
using GoogleMapsApi.Test.Fixtures;
using Xunit;
using Status = GoogleMapsApi.Core.Entities.Geocoding.Response.Status;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class GeocodingTests
    {
        private readonly IntegrationTestFixture _fixture;

        public GeocodingTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Geocoding_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest
            {
                ApiKey = _fixture.ApiKey,
                Address = "285 Bedford Ave, Brooklyn, NY 11211, USA"
            };

            var result = GoogleMaps.Geocode.QueryAsync(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            Assert.True(Regex.IsMatch(result.Results.First().Geometry.Location.LocationString, "40\\.\\d*,-73\\.\\d*"));
        }


        [Fact]
        public void GeocodingAsync_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

            var result = GoogleMaps.Geocode.QueryAsync(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            Assert.True(Regex.IsMatch(result.Results.First().Geometry.Location.LocationString, "40\\.\\d*,-73\\.\\d*"));
        }

        [Fact]
        public void Geocoding_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

            Assert.Throws<AuthenticationException>(() => GoogleMaps.Geocode.QueryAsync(request).Result);
        }

        [Fact]
        public void GeocodingAsync_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };
            
            var exeption = Assert.Throws<AggregateException>(() => GoogleMaps.Geocode.QueryAsync(request).Wait());
            Assert.IsType<AuthenticationException>(exeption.InnerException);
        }

        [Fact]
        public void GeocodingAsync_Cancel_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

            var tokeSource = new CancellationTokenSource();
            var task = GoogleMaps.Geocode.QueryAsync(request, tokeSource.Token);
            tokeSource.Cancel();
            
            var exeption = Assert.Throws<AggregateException>(() => task.Wait());
            Assert.IsType<TaskCanceledException>(exeption.InnerException);
        }

        [Fact]
        public void GeocodingAsync_WithPreCanceledToken_Cancels()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var task = GoogleMaps.Geocode.QueryAsync(request, cts.Token);

            var exeption = Assert.Throws<AggregateException>(() => task.Wait());
            Assert.IsType<TaskCanceledException>(exeption.InnerException);
        }

        [Fact]
        public void ReverseGeocoding_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest { Location = new Location(40.7141289, -73.9614074) };

            var result = GoogleMaps.Geocode.QueryAsync(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Fact]
        public void ReverseGeocodingAsync_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest { Location = new Location(40.7141289, -73.9614074) };

            var result = GoogleMaps.Geocode.QueryAsync(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(Status.OK, result.Status);
            Assert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }
    }
}
