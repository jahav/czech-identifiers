using NodaTime.Text;
using Xunit;

namespace Identifiers.Czech.Tests
{
    internal static class PatternAsserts
    {
        public static void AssertBirthNumber(this ParseResult<BirthNumber> parseResult, int expectedYearPart, int expectedMonthPart, int expectedDayPart, int expectedSequence, int? expectedCheckDigit)
        {
            Assert.True(parseResult.Success);
            var birthNumber = parseResult.Value;
            Assert.Equal(expectedYearPart, birthNumber.YearPart);
            Assert.Equal(expectedMonthPart, birthNumber.MonthPart);
            Assert.Equal(expectedDayPart, birthNumber.DayPart);
            Assert.Equal(expectedSequence, birthNumber.Sequence);
            Assert.Equal(expectedCheckDigit, birthNumber.CheckDigit);
        }

        public static void AssertBirthNumberError<T>(this ParseResult<BirthNumber> parseResult)
        {
            Assert.False(parseResult.Success);
            Assert.Equal(typeof(T), parseResult.Exception.GetType());
        }

        public static void AssertIdentificationNumber(this ParseResult<IdentificationNumber> parseResult, long expectedNumber, int expectedCheckDigit)
        {
            Assert.True(parseResult.Success);
            var birthNumber = parseResult.Value;
            Assert.Equal(expectedNumber, birthNumber.Number);
            Assert.Equal(expectedCheckDigit, birthNumber.CheckDigit);
        }
    }
}
