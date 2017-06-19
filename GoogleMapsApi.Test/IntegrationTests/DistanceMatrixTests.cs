using System;
using System.Linq;
using GoogleMapsApi.Core;
using GoogleMapsApi.Core.Engine;
using GoogleMapsApi.Core.Entities.Directions.Response;
using GoogleMapsApi.Core.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Test.Fixtures;
using Xunit;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class DistanceMatrixTests
    {
        private readonly IntegrationTestFixture _fixture;

        public DistanceMatrixTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ShouldReturnValidValueWhenOneOriginAndOneDestinationsSpeciefed()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" }
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(DirectionsStatusCodes.OK, result.Status);
            Assert.Equal(new[] {"Alter Sirksfelder Weg 7, 23881 Koberg, Germany"}, result.DestinationAddresses);
            Assert.Equal(new[] {"Pilsener Str. 18, 92726 Waidhaus, Germany"}, result.OriginAddresses);
            Assert.Equal(DirectionsStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.NotNull(result.Rows.First().Elements.First().Distance);
            Assert.NotNull(result.Rows.First().Elements.First().Duration);
        }

        [Fact]
        public void ShouldReturnValidValueWhenTwoOriginsSpecified()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088", "49.17395,12.87028" },
                Destinations = new[] { "53.64308,10.52726" }
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(DirectionsStatusCodes.OK, result.Status);
            Assert.Equal(new[] {"Alter Sirksfelder Weg 7, 23881 Koberg, Germany"}, result.DestinationAddresses);
            Assert.Equal(new[] {"Pilsener Str. 18, 92726 Waidhaus, Germany", "Böhmerwaldstraße 19, 93444 Bad Kötzting, Germany"}, result.OriginAddresses);
            Assert.Equal(2, result.Rows.Count());
            Assert.Equal(DirectionsStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.Equal(DirectionsStatusCodes.OK, result.Rows.Last().Elements.First().Status);
        }

        [Fact]
        public void ShouldReturnDurationInTrafficWhenDepartureTimeAndApiKeySpecified()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                DepartureTime = new Time(),
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.Equal(DirectionsStatusCodes.OK, result.Status);

            Assert.NotNull(result.Rows.First().Elements.First().DurationInTraffic);
        }

        [Fact]
        public void ShouldThrowExceptionWhenDepartureTimeAndArrivalTimeSpecified()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                DepartureTime = new Time(),
                ArrivalTime = new Time(),
                Mode = DistanceMatrixTravelModes.transit,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Fact]
        public void ShouldThrowExceptionWhenArrivalTimeSpecifiedForNonTransitModes()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                ArrivalTime = new Time(),
                Mode = DistanceMatrixTravelModes.driving,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Fact]
        public void ShouldThrowExceptionWheTransitRoutingPreferenceSpecifiedForNonTransitModes()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TransitRoutingPreference = DistanceMatrixTransitRoutingPreferences.less_walking,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Fact]
        public void ShouldThrowExceptionWhenTrafficModelSuppliedForNonDrivingMode()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Mode = DistanceMatrixTravelModes.transit,
                DepartureTime = new Time(),
                TrafficModel = DistanceMatrixTrafficModels.optimistic,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Fact]
        public void ShouldThrowExceptionWhenTrafficModelSuppliedWithoutDepartureTime()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TrafficModel = DistanceMatrixTrafficModels.optimistic,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Fact]
        public void ShouldThrowExceptionWhenTransitModesSuppliedForNonTransitMode()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TransitModes = new DistanceMatrixTransitModes[] { DistanceMatrixTransitModes.bus, DistanceMatrixTransitModes.subway},
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Fact]
        public void ShouldReturnImperialUnitsIfImperialPassedAsParameter()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Units = DistanceMatrixUnitSystems.imperial,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
            Assert.True(result.Rows.First().Elements.First().Distance.Text.Contains("mi"));
        }

        [Fact]
        public void ShouldReplaceUriViaOnUriCreated()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            UriCreatedDelegate onUriCreated = delegate (Uri uri)
                {
                    var builder = new UriBuilder(uri);
                    builder.Query = builder.Query.Replace("placeholder", "1,2");
                    return builder.Uri;
                };

            GoogleMaps.DistanceMatrix.OnUriCreated += onUriCreated;

            try
            {
                var result = GoogleMaps.DistanceMatrix.Query(request);
                if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                    Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
                Assert.Equal(DirectionsStatusCodes.OK, result.Status);
                Assert.Equal("1,2", result.OriginAddresses.First());
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnUriCreated -= onUriCreated;
            }
        }

        [Fact]
        public void ShouldPassRawDataToOnRawResponseRecivied()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = _fixture.ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            var rawData = new byte[0];

            RawResponseReciviedDelegate onRawResponseRecivied = data => rawData = data;
            GoogleMaps.DistanceMatrix.OnRawResponseRecivied += onRawResponseRecivied;

            try
            {
                var result = GoogleMaps.DistanceMatrix.Query(request);
                if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                    Assert.True(false, "Cannot run test since you have exceeded your Google API query limit.");
                Assert.Equal(DirectionsStatusCodes.OK, result.Status);
                Assert.NotEmpty(rawData);
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnRawResponseRecivied -= onRawResponseRecivied;
            }
        }
    }
}
