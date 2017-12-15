using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class AccountNumberTest
    {
        [Fact]
        public void StandardFormatConstructor_BankCode_CantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AccountNumber(0, 0, null));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(10, false)]
        [InlineData(19, true)]
        [InlineData(9000000001, true)]
        [InlineData(5000000000, false)]
        [InlineData(400000, false)]
        public void NumberMustHaveAtLeast2NonZeroDigitsToBeValid(long number, bool isValid)
        {
            Assert.Equal(isValid, new AccountNumber(0, number, "0100").IsValid);
        }

        [Theory]
        [InlineData(19, 11, true)]
        [InlineData(742418, 132, true)]
        [InlineData(00483, 35, false)]
        [InlineData(575427, 152, false)]
        [InlineData(58509, 118, false)]
        [InlineData(73, 17, false)]
        public void PrefixChecksumMustBeFullyDivisibleBy11ToBeValid(long prefix, int prefixChecksum, bool isValid)
        {
            var accountNumber = new AccountNumber(prefix, 19, "0100");
            Assert.Equal(accountNumber.PrefixChecksum % 11 == 0, isValid);
            Assert.Equal(prefixChecksum, accountNumber.PrefixChecksum);
            Assert.Equal(isValid, accountNumber.IsValid);
        }

        [Theory]
        [InlineData(765178, 166, false)]
        [InlineData(37, 13, false)]
        [InlineData(78, 22, true)]
        [InlineData(9999999999, 495, true)]
        public void NumberChecksumMustBeFullyDivisibleBy11ToBeValid(long number, int numberChecksum, bool isValid)
        {
            var accountNumber = new AccountNumber(0, number, "0100");
            Assert.Equal(accountNumber.NumberChecksum % 11 == 0, isValid);
            Assert.Equal(numberChecksum, accountNumber.NumberChecksum);
            Assert.Equal(isValid, accountNumber.IsValid);
        }

        [Fact]
        public void Prefix_WithMoreThan6Digits_IsNotValid()
        {
            var sevenDigitCheckSumOkNumber = 1234562;
            var accountNumber = new AccountNumber(sevenDigitCheckSumOkNumber, 19, "0100");
            Assert.False(accountNumber.IsValid);
        }

        [Fact]
        public void Number_WithNumberMoreThan10Digits_IsNotValid()
        {
            var elevenDigitCheckSumOkNumber = 12345678908L;
            var accountNumber = new AccountNumber(0, elevenDigitCheckSumOkNumber, "0100");
            Assert.False(accountNumber.IsValid);
        }

        [Fact]
        public void Prefix_WithNegativeValue_IsNotValid()
        {
            var negativePrefixWithOkCheckuu = -19;
            var accountNumber = new AccountNumber(negativePrefixWithOkCheckuu, 19, "0100");
            Assert.False(accountNumber.IsValid);
        }

        [Fact]
        public void Number_WithNegativeValue_IsNotValid()
        {
            var negativeNumberWithOkChecksum = -19;
            var accountNumber = new AccountNumber(0, negativeNumberWithOkChecksum, "0100");
            Assert.False(accountNumber.IsValid);
        }

    }
}
