using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class IdentifierNumberTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(5001)]
        [InlineData(9999999)]
        public void StandardFormConstructorAcceptsNumberFrom0To9999999(int number)
        {
            new IdentificationNumber(number, 0);
        }

        [Theory]
        [InlineData(-1854)]
        [InlineData(-1)]
        [InlineData(10000000)]
        [InlineData(999999999)]
        public void StandardFormConstructorThrowOutOfRangeOnNumberOutside0To9999999(int number)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentificationNumber(number, 0));
        }

        [Theory]
        [InlineData(125)]
        public void NumberFromStandardFormConstructorCanBeRetrievedFromProperty(int number)
        {
            var idNumber = new IdentificationNumber(number, 0);
            Assert.Equal(number, idNumber.Number);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(9)]
        public void StandardFormConstructorAcceptsCheckDigitFrom0To9(int checkDigit)
        {
            new IdentificationNumber(0, checkDigit);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        [InlineData(99)]
        public void StandardFormConstructorThrowOutOfRangeOnCheckDigitOutside0To9(int checkDigit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentificationNumber(0, checkDigit));
        }

        [Theory]
        [InlineData(4)]
        public void CheckDigitFromStandardFormConstructorCanBeRetrievedFromProperty(int checkDigit)
        {
            var idNumber = new IdentificationNumber(0, checkDigit);
            Assert.Equal(checkDigit, idNumber.CheckDigit);
        }

        [Theory]
        [InlineData(706, 3, 4)]
        public void ValidNumberMustHaveCorrectChecksum(long number, int checkDigit, int expectedCheckDigit)
        {
            var invalidIdNumber = new IdentificationNumber(number, checkDigit);
            Assert.False(invalidIdNumber.IsValid);
            Assert.Equal(expectedCheckDigit, invalidIdNumber.ExpectedCheckDigit);
        }

        [Theory]
        [InlineData(6966396, 9, 3)]
        [InlineData(694, 0, 7)]
        public void ExpectedCheckDigitIsCalculateAsWeightedSumModulo11(long invalidIdNumber, int checkDigit, int expectedCheckDigit)
        {
            var idNumber = new IdentificationNumber(invalidIdNumber, checkDigit);
            Assert.False(idNumber.IsValid, "Last digit is not correct.");
            Assert.Equal(expectedCheckDigit, idNumber.ExpectedCheckDigit);
        }
    }
}
