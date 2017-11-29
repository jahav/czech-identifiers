using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class BirthNumberStandardPatternTest
    {
        private BirthNumberPattern pattern = BirthNumberPattern.StandardPattern;

        [Fact]
        public void PatternDoesntAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => pattern.Parse(null));
        }

        [Fact]
        public void PatternAcceptsBirthNumbersBefore1954()
        {
            pattern.Parse("123456/789").AssertBirthNumber(12, 34, 56, 789, null);
        }

        [Fact]
        public void PatternAcceptsBirthNumbersAfter1954()
        {
            pattern.Parse("123456/7895").AssertBirthNumber(12, 34, 56, 789, 5);
        }

        [Fact]
        public void PatternDoesntAcceptTextLongerThan10Digits()
        {
            Assert.Throws<FormatException>(() => pattern.Parse("1234567/8901").GetValueOrThrow());
        }

        [Fact]
        public void PatternDoesntAcceptTextShorterThan9Digits()
        {
            Assert.Throws<FormatException>(() => pattern.Parse("123456").GetValueOrThrow());
        }

        [Fact]
        public void PatternDoesntAcceptNonDigits()
        {
            Assert.Throws<FormatException>(() => pattern.Parse("ab1099/0300").GetValueOrThrow());
        }
    }
}
