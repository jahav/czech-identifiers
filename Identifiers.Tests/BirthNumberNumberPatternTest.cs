using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class BirthNumberNumberPatternTest
    {
        private BirthNumberPattern pattern = BirthNumberPattern.NumberPattern;

        [Fact]
        public void PatternAcceptsBirthNumbersBefore1954()
        {
            pattern.Parse("010203004").VerifyBirthNumber(1, 2, 3, 4, null);
        }

        [Fact]
        public void PatternAcceptsBirthNumbersAfter1954()
        {
            pattern.Parse("010203004").VerifyBirthNumber(1, 2, 3, 4, null);
        }

        [Fact]
        public void PatternDoesntAcceptStandardPattern()
        {
            pattern.Parse("010203/004").VerifyBirthNumberError<FormatException>();
        }

        [Theory]
        [InlineData("abcd02456")]
        [InlineData("12345678901")]
        [InlineData("00000000000")]
        [InlineData("00000")]
        public void PatternDoesntAcceptText(string text)
        {
            pattern.Parse(text).VerifyBirthNumberError<FormatException>();
        }

    }
}
