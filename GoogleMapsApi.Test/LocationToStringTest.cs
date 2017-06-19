using GoogleMapsApi.Core.Entities.Common;
using Xunit;

namespace GoogleMapsApi.Test
{
    public class LocationToStringTest
    {
        [Fact]
        public void WhenNearZeroLongitude_ExpectCorrectToString()
        {
            // Longitude of 0.000009 is converted to 9E-06 using Invariant ToString, but we need 0.000009
            var location = new Location(57.231d, 0.000009d);
            Assert.Equal("57.231,0.000009", location.ToString());
        }

        [Fact]
        public void WhenZeroLongitude_ExpectCorrectToString()
        {
            // Longitude of 0.000009 is converted to 9E-06 using Invariant ToString, but we need 0.000009
            var location = new Location(52.123123d, 0.0d);
            Assert.Equal("52.123123,0.0", location.ToString());
        }
    }
}
