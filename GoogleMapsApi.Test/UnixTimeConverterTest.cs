using System;
using GoogleMapsApi.Engine;
using Xunit;

namespace GoogleMapsApi.Test
{
    public class UnixTimeConverterTest
    {
        [Fact]
        public void DateTimeToUnixTimestamp_Zero_ExpectedResult()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            const int expected = 0;
            Assert.Equal(expected, UnixTimeConverter.DateTimeToUnixTimestamp(epoch));
            Assert.Equal(expected, UnixTimeConverter.DateTimeToUnixTimestamp(epoch.ToLocalTime()));
        }

        [Fact]
        public void DateTimeToUnixTimestamp_DST_ExpectedResult()
        {
            var dst = new DateTime(2016, 4, 4, 10, 0, 0, DateTimeKind.Utc);
            const int expected = 1459764000;
            Assert.Equal(expected, UnixTimeConverter.DateTimeToUnixTimestamp(dst));
            Assert.Equal(expected, UnixTimeConverter.DateTimeToUnixTimestamp(dst.ToLocalTime()));
        }

        [Fact]
        public void DateTimeToUnixTimestamp_NonDST_ExpectedResult()
        {
            var nonDst = new DateTime(2016, 3, 1, 11, 0, 0, DateTimeKind.Utc);
            const int expected = 1456830000;
            Assert.Equal(expected, UnixTimeConverter.DateTimeToUnixTimestamp(nonDst));
            Assert.Equal(expected, UnixTimeConverter.DateTimeToUnixTimestamp(nonDst.ToLocalTime()));
        }
    }
}