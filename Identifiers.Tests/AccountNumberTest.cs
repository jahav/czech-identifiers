using Xunit;

namespace Identifiers.Czech.Tests
{
    public class AccountNumberTest
    {
        [Fact]
        public void NullDoesntHaveStandardForm()
        {
            Assert.False(new AccountNumber(null).HasStandardFormat);
        }

        [Theory]
        [InlineData("0012/0300", 12, "0300")]
        [InlineData("1234567890/6200", 1234567890, "6200")]
        [InlineData("0000/6200", 0, "6200")]
        public void CanAcceptAccountWithoutPrefix(string accountNumber, int? expectedNumber, string expectedBankCode)
        {
            var account = new AccountNumber(accountNumber);
            Assert.True(account.HasStandardFormat);
            Assert.Equal(expectedNumber, account.Number);
            Assert.Equal(expectedBankCode, account.BankCode);
        }

        [Theory]
        [InlineData("17-0012/0300", 17, 12, "0300")]
        [InlineData("000-1234567890/6200", 0, 1234567890, "6200")]
        public void CanAcceptAccountWithPrefix(string accountNumber, int? expectedPrefix, int? expectedNumber, string expectedBankCode)
        {
            var account = new AccountNumber(accountNumber);
            Assert.True(account.HasStandardFormat);
            Assert.Equal(expectedPrefix, account.Prefix);
            Assert.Equal(expectedNumber, account.Number);
            Assert.Equal(expectedBankCode, account.BankCode);
        }

        [Theory]
        [InlineData("1234567-12/0100")]
        [InlineData("abcdef-12/0100")]
        public void PrefixMustBe0To6DigitsToBeInStandardForm(string invalidInput)
        {
            var invalidAccount = new AccountNumber(invalidInput);
            Assert.False(invalidAccount.HasStandardFormat);
            Assert.Null(invalidAccount.Prefix);
            Assert.Null(invalidAccount.Number);
            Assert.Null(invalidAccount.BankCode);
        }

        [Theory]
        [InlineData("1/0100")]
        [InlineData("12345678901/0100")]
        [InlineData("abc/0100")]
        [InlineData("a2/0100")]
        public void AccountNumberMustBe2To10DigitsToBeInStandardForm(string invalidInput)
        {
            var invalidAccount = new AccountNumber(invalidInput);
            Assert.False(invalidAccount.HasStandardFormat);
            Assert.Null(invalidAccount.Prefix);
            Assert.Null(invalidAccount.Number);
            Assert.Null(invalidAccount.BankCode);
        }

        [Theory]
        [InlineData("12/123")]
        [InlineData("12/12345")]
        [InlineData("12/abcd")]
        [InlineData("12/A100")]
        public void BankCode4DigitsToBeInStandardForm(string invalidInput)
        {
            var invalidAccount = new AccountNumber(invalidInput);
            Assert.False(invalidAccount.HasStandardFormat);
            Assert.Null(invalidAccount.Prefix);
            Assert.Null(invalidAccount.Number);
            Assert.Null(invalidAccount.BankCode);
        }

        [Theory]
        [InlineData("00/0100", false)]
        [InlineData("10/0100", false)]
        [InlineData("19/0100", true)]
        [InlineData("9000000001/0100", true)]
        [InlineData("5000000000/0100", false)]
        [InlineData("000400000/0100", false)]
        public void NumberMustHaveAtLeast2NonZeroDigitsToBeValid(string account, bool isValid)
        {
            Assert.Equal(isValid, new AccountNumber(account).IsValid);
        }

        [Theory]
        [InlineData("19-19/0100", 11, true)]
        [InlineData("742418-19/0100", 132, true)]
        [InlineData("00483-19/0000", 35, false)]
        [InlineData("575427-19/0000", 152, false)]
        [InlineData("58509-19/0000", 118, false)]
        [InlineData("73-19/0000", 17, false)]
        public void PrefixChecksumMustBeFullyDivisibleBy11ToBeValid(string account, int prefixChecksum, bool isValid)
        {
            var accountNumber = new AccountNumber(account);
            Assert.Equal(accountNumber.PrefixChecksum % 11 == 0, isValid);
            Assert.Equal(prefixChecksum, accountNumber.PrefixChecksum);
            Assert.Equal(isValid, accountNumber.IsValid);
        }

        [Theory]
        [InlineData("765178/0100", 166, false)]
        [InlineData("37/0100", 13, false)]
        [InlineData("78/0100", 22, true)]
        [InlineData("9999999999/0100", 495, true)]
        public void NumberChecksumMustBeFullyDivisibleBy11ToBeValid(string account, int numberChecksum, bool isValid)
        {
            var accountNumber = new AccountNumber(account);
            Assert.Equal(accountNumber.NumberChecksum % 11 == 0, isValid);
            Assert.Equal(numberChecksum, accountNumber.NumberChecksum);
            Assert.Equal(isValid, accountNumber.IsValid);
        }
    }
}
