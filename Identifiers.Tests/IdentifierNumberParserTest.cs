using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class IdentifierNumberParserTest
    {
        private IdentifierNumberParser parser = new IdentifierNumberParser();

        [Fact]
        public void ParserDoesntAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Fact]
        public void InputLongerThan8DigitsThrowsFormatException()
        {
            var idNumber9digits = "000007064";
            Assert.Throws<FormatException>(() => parser.Parse(idNumber9digits));
        }

        [Fact]
        public void InputShorterThan8DigitsThrowsFormatException()
        {
            var idNumber7digits = "0007064";
            Assert.Throws<FormatException>(() => parser.Parse(idNumber7digits));
        }

        [Fact]
        public void Input8DigitsLongWillBeParsed()
        {
            var idNumber8digits = "00007064";
            var idNum = parser.Parse(idNumber8digits);
            Assert.Equal(706, idNum.Number);
            Assert.Equal(4, idNum.CheckDigit);
        }

        [Theory]
        [InlineData("abcd0123")]
        [InlineData("abcd")]
        public void InputOtherThan8DigitsWillThrowFormatException(string invalidNumber)
        {
            Assert.Throws<FormatException>(() => parser.Parse(invalidNumber));
        }
    }
}
