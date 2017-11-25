using System;
using Xunit;

namespace Identifiers.Tests
{
    public class BirthNumberTest
    {
        [Fact]
        public void NullDoesntHaveStandardForm()
        {
            Assert.False(new BirthNumber(null).HasStandardForm);
        }

        [Fact]
        public void Number9Or10DigitsHaveStandardForm()
        {
            Assert.True(new BirthNumber("123456789").HasStandardForm);
            Assert.True(new BirthNumber("1234567890").HasStandardForm);
        }

        [Fact]
        public void NumberLongerThan10DigitsDoesntHaveStandardForm()
        {
            Assert.False(new BirthNumber("12345678901").HasStandardForm);
        }

        [Fact]
        public void NumberShorterThan9DigitsDoesntHaveStandardForm()
        {
            Assert.False(new BirthNumber("123456").HasStandardForm);
        }

        [Theory]
        [InlineData("09004264555", false)]
        [InlineData("9004264555", true)]
        public void NumberIsValidOnlyIfHasStandardForm(string birthNumber, bool shouldBeValid)
        {
            Assert.Equal(shouldBeValid, new BirthNumber(birthNumber).IsValid);
        }

        [Theory]
        [InlineData("505504237", true)]
        [InlineData("355105307", true)]
        [InlineData("505500001", false)]
        [InlineData("530004001", false)]
        [InlineData("539904001", false)]
        [InlineData("530599001", false)]
        public void DatePartMustBeValidDateForValidNumber(string birthNumber, bool shouldBeValid)
        {
            Assert.Equal(shouldBeValid, new BirthNumber(birthNumber).IsValid);
        }

        [Theory]
        [InlineData("505504237", 5)]
        [InlineData("126224452", 12)]
        [InlineData("355001001", null)]
        [InlineData("455008001", null)]
        [InlineData("125131001", 1)]
        public void MonthPartHasAdded50ForFermales(string birthNumber, int? birthMonth)
        {
            var determinedMonth = IgnoreOutsideRange(new BirthNumber(birthNumber).Month);
            Assert.Equal(birthMonth, determinedMonth);
        }

        [Theory]
        [InlineData("032101001", null)]
        [InlineData("037101001", null)]
        [InlineData("0321010010", null)]
        [InlineData("0371010010", null)]
        [InlineData("0420010010", null)]
        [InlineData("0421010010", 1)]
        [InlineData("0429010010", 9)]
        [InlineData("0432010010", 12)]
        [InlineData("0433010010", null)]
        [InlineData("0470010010", null)]
        [InlineData("0471010010", 1)]
        [InlineData("0479010010", 9)]
        [InlineData("0482010010", 12)]
        [InlineData("0483010010", null)]
        public void MonthPartCanHave20AddedWhenAllocationExhausedAfterYear2003(string birthNumber, int? expectedBirthMonth)
        {
            var isOutsideRange = expectedBirthMonth == null;
            var determinedMonth = IgnoreOutsideRange(new BirthNumber(birthNumber).Month);
            Assert.Equal(expectedBirthMonth, determinedMonth);
        }

        [Theory]
        [InlineData("540101001", 1854)]
        [InlineData("990101001", 1899)]
        [InlineData("000101001", 1900)]
        [InlineData("530101001", 1953)]
        [InlineData("5401010010", 1954)]
        [InlineData("9901010010", 1999)]
        [InlineData("0001010010", 2000)]
        [InlineData("1701010010", 2017)]
        [InlineData("5301010010", 2053)]
        public void YearIsAccuratelyDetermined(string birthNumber, int expectedYear)
        {
            Assert.Equal(expectedYear, new BirthNumber(birthNumber).Year);
        }

        public void YearIsNullIfNumberIsNotInStandardForm()
        {
            Assert.Null(new BirthNumber("123456").Year);
        }

        [Theory]
        [InlineData("5401010010", 0)]
        [InlineData("6503192641", 1)]
        [InlineData("9559121902", 2)]
        [InlineData("0151194483", 3)]
        [InlineData("7953229944", 4)]
        [InlineData("9811085515", 5)]
        [InlineData("5552242696", 6)]
        [InlineData("0004266207", 7)]
        [InlineData("6759141488", 8)]
        [InlineData("5455285869", 9)]
        [InlineData("7801233540", 0)]
        public void ExpectedCheckDigitIsSumModulo11(string birthNumber, int expectedCheckDigit)
        {
            Assert.Equal(expectedCheckDigit, new BirthNumber(birthNumber).ExpectedCheckDigit);
        }

        [Theory]
        [InlineData("530101001")]
        [InlineData("700101001")]
        public void ExpectedCheckDigitIsNullBefore1954(string birthNumber)
        {
            Assert.Null(new BirthNumber(birthNumber).ExpectedCheckDigit);
        }

        [Theory]
        [InlineData("0000")]
        [InlineData("0xDEADBEEF")]
        public void ExpectedCheckDigitIsNullInNumbersWithoutStandardForm(string birthNumber)
        {
            Assert.Null(new BirthNumber(birthNumber).ExpectedCheckDigit);
        }

        [Theory]
        [InlineData("0001010008", false)]
        [InlineData("0001010009", true)]
        [InlineData("0001010010", true)]
        [InlineData("0001010011", false)]
        [InlineData("5412242561", true)]
        [InlineData("7552318510", false)]

        public void NumberAfter1954IsValidOnlyIfItIsHasStandardFormAndValidDatePartAndCheckDigit(string birthNumber, bool shouldBeValid)
        {
            Assert.Equal(shouldBeValid, new BirthNumber(birthNumber).IsValid);
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
