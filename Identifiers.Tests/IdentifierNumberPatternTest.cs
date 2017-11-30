using System;
using System.Text;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class IdentifierNumberPatternTest
    {
        private IdentificationNumberPattern pattern = IdentificationNumberPattern.StandardPattern;

        [Fact]
        public void PatternDoesntAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => pattern.Parse(null).GetValueOrThrow());
        }

        [Fact]
        public void PatternDoesntAcceptTextLongerThan8Characters()
        {
            var idNumber9digits = "000007064";
            Assert.Throws<FormatException>(() => pattern.Parse(idNumber9digits).GetValueOrThrow());
        }

        [Fact]
        public void PatternDoesntAcceptTextShorterThan8Characters()
        {
            var idNumber7digits = "0007064";
            Assert.Throws<FormatException>(() => pattern.Parse(idNumber7digits).GetValueOrThrow());
        }

        [Fact]
        public void PatternAccepts8DigitText()
        {
            var idNumber8digits = "00007064";
            pattern.Parse(idNumber8digits).AssertIdentificationNumber(706, 4);
        }

        [Fact]
        public void PatternAcceptsMaximalIco()
        {
            var idNumber8digits = "99999999";
            pattern.Parse(idNumber8digits).AssertIdentificationNumber(9999999L, 9);
        }

        [Theory]
        [InlineData("abcd0123")]
        [InlineData("abcd")]
        public void TextOtherThan8DigitsWillNotBeAccepted(string invalidNumber)
        {
            Assert.Throws<FormatException>(() => pattern.Parse(invalidNumber).GetValueOrThrow());
        }

        [Fact]
        public void Format_WillFormatIdentificationNumberInStandardPattern()
        {
            var identificationNumber = new IdentificationNumber(123, 5);
            Assert.Equal(identificationNumber.ToString("S", null), pattern.Format(identificationNumber));
        }

        [Fact]
        public void ApendBuilderCheckThatBuilderIsNotNull()
        {
            var identificationNumber = new IdentificationNumber(0, 0);
            Assert.Throws<ArgumentNullException>(() => pattern.AppendFormat(identificationNumber, null));
        }

        [Fact]
        public void ApendBuilderAppendsIdentificationNumberInStandardPattern()
        {
            var identificationNumber = new IdentificationNumber(123, 0);
            var standardFormat = identificationNumber.ToString("S", null);
            var builder = new StringBuilder("Hello, my ICO is ");
            builder = pattern.AppendFormat(identificationNumber, builder);
            Assert.Equal("Hello, my ICO is 00001230", builder.ToString());
        }
    }
}
