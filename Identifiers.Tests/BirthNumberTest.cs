using System;
using System.Collections.Generic;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class BirthNumberTest
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(99, 99, 99)]
        public void StandardFormConstructor_Accepts0To99InDateParts(int yearPart, int monthPart, int dayPart)
        {
            new BirthNumber(yearPart, monthPart, dayPart, 0, null, string.Empty);
        }

        [Theory]
        [InlineData(-1, 0, 0)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 0, -1)]
        [InlineData(100, 0, 0)]
        [InlineData(0, 100, 0)]
        [InlineData(0, 0, 100)]
        public void StandardFormConstructor_OnNumbersOutside0To99InDatePart_ThrowsOutOfRangeException(int yearPart, int monthPart, int dayPart)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BirthNumber(yearPart, monthPart, dayPart, 0, null, string.Empty));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(99)]
        public void StandardFormConstructor_Accepts0To999InSequence(int sequence)
        {
            new BirthNumber(0, 0, 0, sequence, null, string.Empty);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1000)]
        public void StandardFormConstructor_OnNumbersOutside0To999InSequence_ThrowsOutOfRangeException(int sequence)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BirthNumber(0, 0, 0, sequence, null, string.Empty));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(9)]
        public void StandardFormConstructor_Accepts0To9OrNullInCheckDigit(int? checkDigit)
        {
            new BirthNumber(0, 0, 0, 0, checkDigit, string.Empty);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void StandardFormConstructor_OnNumbersOutside0To9InCheckDigit_ThrowsOutOfRangeException(int checkDigit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BirthNumber(0, 0, 0, 0, checkDigit, string.Empty));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("846517/0014")]
        public void StandardFormConstructor_AcceptsAnythingInInput(string input)
        {
            var birthNumber = new BirthNumber(0, 0, 0, 0, 0, input);
            Assert.Equal(input, birthNumber.Input);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 5)]
        [InlineData(12, 12)]
        [InlineData(51, 1)]
        [InlineData(57, 7)]
        [InlineData(62, 12)]
        public void MonthPartCanBeShiftedBy50ForWomen(int monthPart, int expectedMonth)
        {
            var birthNumber = new BirthNumber(0, monthPart, 1, 0, 0, null);
            Assert.Equal(expectedMonth, birthNumber.Month);
        }

        [Theory]
        [InlineData(21, 1)]
        [InlineData(29, 9)]
        [InlineData(32, 12)]
        [InlineData(71, 1)]
        [InlineData(74, 4)]
        [InlineData(82, 12)]
        public void MonthPartCanBeShiftedBy20WhenSequenceIsExhaustedAfterYear2003(int monthPart, int expectedMonth)
        {
            var birthNumber = new BirthNumber(04, monthPart, 1, 0, 0, null);
            Assert.Equal(expectedMonth, birthNumber.Month);
        }

        [Theory]
        [MemberData(nameof(GetInvalidMonthPartsAfter2003))]
        [MemberData(nameof(GetInvalidMonthPartsBefore2004))]
        public void BirthNumberWithInvalidMonthPartWillBeOutOf1To12Range(int year, int monthPart)
        {
            var birthNumber = new BirthNumber(year, monthPart, 1, 0, 0, null);
            Assert.NotInRange(birthNumber.Month, 1, 12);
        }

        [Theory]
        [MemberData(nameof(GetValidMonthPartsAfter2003))]
        [MemberData(nameof(GetValidMonthPartsBefore2004))]
        public void BirthNumberWithValidMonthPartWillBeIn1To12Range(int year, int monthPart)
        {
            var birthNumber = new BirthNumber(year, monthPart, 1, 0, 0, null);
            Assert.InRange(birthNumber.Month, 1, 12);
        }

        [Theory]
        [MemberData(nameof(GetAllYears))]
        public void BirthNumberCorrectlyDeterminesYear(int yearPart, bool hasCheckDigit, int expectedYear)
        {
            var birthNumber = new BirthNumber(yearPart, 1, 1, 1, hasCheckDigit ? 1 : (int?)null, string.Empty);
            Assert.Equal(expectedYear, birthNumber.Year);
        }

        [Theory]
        [InlineData(54, 01, 01, 001, 0)]
        [InlineData(65, 03, 19, 264, 1)]
        [InlineData(95, 59, 12, 190, 2)]
        [InlineData(01, 51, 19, 448, 3)]
        [InlineData(79, 53, 22, 994, 4)]
        [InlineData(98, 11, 08, 551, 5)]
        [InlineData(55, 52, 24, 269, 6)]
        [InlineData(00, 04, 26, 620, 7)]
        [InlineData(67, 59, 14, 148, 8)]
        [InlineData(54, 55, 28, 586, 9)]
        [InlineData(78, 01, 23, 354, 0)]
        public void ExpectedCheckDigitIsCalculatedAsNumberWithoutCheckDigitModulo11(int yearPart, int monthPart, int dayPart, int sequence, int expectedCheckDigit)
        {
            Assert.Equal(expectedCheckDigit, new BirthNumber(yearPart, monthPart, dayPart, sequence, 5, string.Empty).ExpectedCheckDigit);
        }

        [Fact]
        public void ExpectedCheckDigit_ForBirthNumbersBefore1954_IsNull()
        {
            Assert.Null(new BirthNumber(50, 1, 1, 1, null, string.Empty).ExpectedCheckDigit);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        [InlineData(99)]
        public void Day_ForStandardFormConstructor_IsEqualToDayPart(int day)
        {
            Assert.Equal(day, new BirthNumber(50, 1, day, 1, null, string.Empty).Day);
        }

        [Theory]
        [InlineData(50, 55, 04, true)]
        [InlineData(35, 51, 05, true)]
        [InlineData(50, 62, 31, true)]
        [InlineData(53, 00, 04, false)]
        [InlineData(53, 99, 04, false)]
        [InlineData(53, 05, 99, false)]
        public void DatePartMustBeValidDateForValidNumber(int yearPart, int monthPart, int dayPart, bool shouldBeValid)
        {
            Assert.Equal(shouldBeValid, new BirthNumber(yearPart, monthPart, dayPart, 1, null, string.Empty).IsValid);
        }

        [Theory]
        [InlineData(54, null, 1854)]
        [InlineData(99, null, 1899)]
        [InlineData(00, null, 1900)]
        [InlineData(53, null, 1953)]
        [InlineData(54, 0, 1954)]
        [InlineData(99, 0, 1999)]
        [InlineData(00, 0, 2000)]
        [InlineData(17, 0, 2017)]
        [InlineData(53, 0, 2053)]
        public void YearIsAccuratelyDetermined(int yearPart, int? checkDigit, int expectedYear)
        {
            Assert.Equal(expectedYear, new BirthNumber(yearPart, 1, 1, 0, checkDigit, string.Empty).Year);
        }

        [Theory]
        [InlineData(53)]
        [InlineData(70)]
        public void ExpectedCheckDigit_Before1954_IsNull(int yearPart)
        {
            Assert.Null(new BirthNumber(yearPart, 1, 1, 0, null, string.Empty).ExpectedCheckDigit);
        }

        [Theory]
        [InlineData(00, 01, 01, 000, 8, false)]
        [InlineData(00, 01, 01, 000, 9, true)]
        [InlineData(00, 01, 01, 001, 0, true)]
        [InlineData(00, 01, 01, 001, 1, false)]
        [InlineData(54, 12, 24, 256, 1, true)]
        [InlineData(75, 52, 31, 851, 0, false)]
        public void BirthNumbersAfter1954_IsValid_OnlyIfItIsHasStandardFormAndValidDatePartAndCheckDigit(int yearPart, int monthPart, int dayPart, int sequence, int checkDigit, bool shouldBeValid)
        {
            var birthNumber = new BirthNumber(yearPart, monthPart, dayPart, sequence, checkDigit, string.Empty);
            Assert.Equal(shouldBeValid, birthNumber.IsValid);
        }

        [Theory]
        [InlineData(00, 01, 01, true)]
        [InlineData(00, 12, 01, true)]
        [InlineData(00, 02, 28, true)]
        [InlineData(00, 13, 01, false)]
        [InlineData(54, 12, 70, false)]
        [InlineData(75, 02, 30, false)]
        public void BirthNumbersBefore1954_IsValid_OnlyIfItIsHasStandardFormAndValidDatePart(int yearPart, int monthPart, int dayPart, bool shouldBeValid)
        {
            var birthNumber = new BirthNumber(yearPart, monthPart, dayPart, 0, null, string.Empty);
            Assert.Equal(shouldBeValid, birthNumber.IsValid);
        }

        public static IEnumerable<object[]> GetAllYears()
        {
            for (int year = 1854; year < 1954; year++)
            {
                yield return new object[] { year % 100, false, year };
            }
            for (int year = 1954; year < 2054; year++)
            {
                yield return new object[] { year % 100, true, year };
            }
        }

        public static IEnumerable<object[]> GetInvalidMonthPartsAfter2003()
        {
            int year = 17;
            yield return new object[] { year, 0 };
            for (int i = 13; i <= 20; i++) yield return new object[] { year, i };
            for (int i = 33; i <= 50; i++) yield return new object[] { year, i };
            for (int i = 63; i <= 70; i++) yield return new object[] { year, i };
            for (int i = 83; i <= 99; i++) yield return new object[] { year, i };
        }

        public static IEnumerable<object[]> GetInvalidMonthPartsBefore2004()
        {
            int year = 01;
            yield return new object[] { year, 0 };
            for (int i = 13; i <= 50; i++) yield return new object[] { year, i };
            for (int i = 63; i <= 99; i++) yield return new object[] { year, i };
        }

        public static IEnumerable<object[]> GetValidMonthPartsAfter2003()
        {
            int year = 17;
            for (int i = 1; i <= 12; i++) yield return new object[] { year, i };
            for (int i = 21; i <= 32; i++) yield return new object[] { year, i };
            for (int i = 51; i <= 62; i++) yield return new object[] { year, i };
            for (int i = 71; i <= 82; i++) yield return new object[] { year, i };
        }

        public static IEnumerable<object[]> GetValidMonthPartsBefore2004()
        {
            int year = 01;
            for (int i = 1; i <= 12; i++) yield return new object[] { year, i };
            for (int i = 51; i <= 62; i++) yield return new object[] { year, i };
        }

    }
}
