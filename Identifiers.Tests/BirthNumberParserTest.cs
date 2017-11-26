using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class BirthNumberParserTest
    {
        private BirthNumberParser parser = new BirthNumberParser();

        [Fact]
        public void ParseDoesntAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }
        
        [Fact]
        public void Input_With9Or10Digits_WillBeParsed()
        {
            parser.Parse("123456789");
            parser.Parse("1234567890");
        }

        [Fact]
        public void Input_LongerThan10Digits_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => parser.Parse("12345678901"));
        }

        [Fact]
        public void Input_ShorterThan9Digits_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => parser.Parse("123456"));
        }

        private int? IgnoreOutsideRange(int? month)
        {
            if (month == null || month < 1 || month > 12)
            {
                return null;
            }

            return month;
        }
    }
}
