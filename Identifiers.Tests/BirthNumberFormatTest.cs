using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class BirthNumberFormatTest
    {
        [Fact]
        public void BirthNumberWithUnspecifiedFormatUsesStandardFormat()
        {
            var birthNumber = new BirthNumber(1, 2, 3, 4, 5, null);
            Assert.Equal("010203/0045", birthNumber.ToString(null, null));
        }

        [Fact]
        public void UseOfUnsupportedFormatThrowsException()
        {
            var birthNumber = new BirthNumber(1, 2, 3, 4, 5, null);
            Assert.Throws<ArgumentException>(() => birthNumber.ToString("unsupported", null));
        }

        [Theory]
        [InlineData("N")]
        [InlineData("n")]
        public void UseOfNumberFormatWillFormatBirthNumberAs9Or10DigitNumber(string numberFormat)
        {
            Assert.Equal("0102030045", new BirthNumber(1, 2, 3, 4, 5, null).ToString(numberFormat, null));
            Assert.Equal("010203004", new BirthNumber(1, 2, 3, 4, null, null).ToString(numberFormat, null));
        }

        [Theory]
        [InlineData("S")]
        [InlineData("s")]
        public void UseOfStandardFormatWillFormatBirthNumberWithSlash(string numberFormat)
        {
            Assert.Equal("010203/0045", new BirthNumber(1, 2, 3, 4, 5, null).ToString(numberFormat, null));
            Assert.Equal("010203/004", new BirthNumber(1, 2, 3, 4, null, null).ToString(numberFormat, null));
        }
    }
}
