using System;
using NodaTime.Text;
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
            pattern.Parse("123456/789").VerifyBirthNumber(12, 34, 56, 789, null);
        }

        [Fact]
        public void PatternAcceptsBirthNumbersAfter1954()
        {
            pattern.Parse("123456/7895").VerifyBirthNumber(12, 34, 56, 789, 5);
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

    internal static class Extensions
    {
        public static void VerifyBirthNumber(this ParseResult<BirthNumber> parseResult, int expectedYearPart, int expectedMonthPart, int expectedDayPart, int expectedSequence, int? expectedCheckDigit)
        {
            Assert.True(parseResult.Success);
            var birthNumber = parseResult.Value;
            Assert.Equal(expectedYearPart, birthNumber.yearPart);
            Assert.Equal(expectedMonthPart, birthNumber.monthPart);
            Assert.Equal(expectedDayPart, birthNumber.dayPart);
            Assert.Equal(expectedSequence, birthNumber.sequence);
            Assert.Equal(expectedCheckDigit, birthNumber.checkDigit);
        }

        public static void VerifyBirthNumberError<T>(this ParseResult<BirthNumber> parseResult)
        {
            Assert.False(parseResult.Success);
            Assert.Equal(typeof(T), parseResult.Exception.GetType());
        }
    }
}
