using System;
using Xunit;

namespace Identifiers.Tests
{
    public class IdentifierNumberTest
    {
        [Fact]
        public void NullIsInvalidNumber()
        {
            Assert.False(new IdentificationNumber(null).IsValid, "Null shouldn't throw an exception, but it is just invalid number.");
        }

        [Fact]
        public void NumberLongerThan8DigitsIsInvalid()
        {
            var idNumber9digits = "000007064";
            var idNum = new IdentificationNumber(idNumber9digits);
            Assert.False(idNum.IsValid, "Although the number itself is valid, it has more than 8 digits.");
        }

        [Fact]
        public void NumberShorterThan8DigitsIsInvalid()
        {
            var idNumber7digits = "0007064";
            var idNum = new IdentificationNumber(idNumber7digits);
            Assert.False(idNum.IsValid, "Although the number itself is valid, it has less than 8 digits.");
        }

        [Fact]
        public void Number8DigitsLongIsValid()
        {
            var idNumber8digits = "00007064";
            var idNum = new IdentificationNumber(idNumber8digits);
            Assert.True(idNum.IsValid, "The number is valid and is 8 digits long.");
        }

        [Fact]
        public void NumberMustHaveCorrectModulo()
        {
            var invalidIdNumber = new IdentificationNumber("00007063");
            Assert.False(invalidIdNumber.IsValid, "The number should be invalid because of modulo.");
        }

        [Theory]
        [InlineData("0006947")]
        [InlineData("0045274649")]
        [InlineData(null)]
        [InlineData("abc")]
        public void CheckDigitIsNullOnNonstandardNumber(string nonStandardNumber)
        {
            Assert.Null(new IdentificationNumber(nonStandardNumber).CheckDigit);
        }

        [Theory]
        [InlineData("00006947", 7)]
        [InlineData("25702556", 6)]
        public void CheckDigitOnStandardNumberIsLastDigit(string standardNumber, int checkDigit)
        {
            Assert.Equal(checkDigit, new IdentificationNumber(standardNumber).CheckDigit);
        }

        [Theory]
        [InlineData("0006947")]
        [InlineData("0045274649")]
        [InlineData(null)]
        [InlineData("abc")]
        public void ExpectedCheckDigitIsNullOnNonstandardNumber(string nonStandardNumber)
        {
            Assert.Null(new IdentificationNumber(nonStandardNumber).ExpectedCheckDigit);
        }

        [Theory]
        [InlineData("69663969", 3)]
        [InlineData("00006940", 7)]
        public void ExpectedCheckDigitIsCalculateCorrectly(string invalidIdNumber, int expectedCheckDigit)
        {
            var idNumber = new IdentificationNumber(invalidIdNumber);
            Assert.False(idNumber.IsValid, "Last digit is not correct.");
            Assert.Equal(expectedCheckDigit, idNumber.ExpectedCheckDigit);
        }
    }
}
