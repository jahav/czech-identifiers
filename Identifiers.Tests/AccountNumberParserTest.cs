using System;
using System.Text;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class AccountNumberParserTest
    {
        private AccountNumberPattern pattern = AccountNumberPattern.StandardPattern;

        [Fact]
        public void PatternDoesntAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => pattern.Parse(null));
        }

        [Theory]
        [InlineData("-12/0100", 0)]
        [InlineData("00123-12/0100", 123)]
        [InlineData("123456-12/0100", 123456)]
        public void Text_WithPrefix0To6Digits_IsAccepted(string accountNumber, long expectedPrefix)
        {
            Assert.Equal(expectedPrefix, pattern.Parse(accountNumber).Value.Prefix);
        }

        [Theory]
        [InlineData("aa-12/0100")]
        [InlineData("0000123-12/0100")]
        [InlineData("abc-12/0100")]
        public void Text_WithoutPrefix0To6Digits_IsNotAccepted(string accountNumber)
        {
            Assert.Throws<FormatException>(() => pattern.Parse(accountNumber).GetValueOrThrow());
        }

        [Theory]
        [InlineData("12/0100", 12)]
        [InlineData("00123/0100", 123)]
        [InlineData("1234567890/0100", 1234567890L)]
        public void Text_WithNumber2To10Digits_IsAccepted(string accountNumber, long expectedNumber)
        {
            Assert.Equal(expectedNumber, pattern.Parse(accountNumber).Value.Number);
        }

        [Theory]
        [InlineData("1/0100")]
        [InlineData("0/0100")]
        [InlineData("12345678901/0100")]
        [InlineData("abd/0100")]
        public void Input_WithoutNumber2To10Digits_ThrowsFormatException(string accountNumber)
        {
            Assert.Throws<FormatException>(() => pattern.Parse(accountNumber).GetValueOrThrow());
        }

        [Theory]
        [InlineData("0012/0300", 12, "0300")]
        [InlineData("1234567890/6200", 1234567890, "6200")]
        [InlineData("0000/6200", 0, "6200")]
        public void CanAcceptAccountWithoutPrefix(string accountNumber, int expectedNumber, string expectedBankCode)
        {
            var account = pattern.Parse(accountNumber).Value;
            Assert.Equal(expectedNumber, account.Number);
            Assert.Equal(expectedBankCode, account.BankCode);
        }

        [Theory]
        [InlineData("17-0012/0300", 17, 12, "0300")]
        [InlineData("000-1234567890/6200", 0, 1234567890, "6200")]
        public void CanAcceptAccountWithPrefix(string accountNumber, int expectedPrefix, int expectedNumber, string expectedBankCode)
        {
            var account = pattern.Parse(accountNumber).Value;
            Assert.Equal(expectedPrefix, account.Prefix);
            Assert.Equal(expectedNumber, account.Number);
            Assert.Equal(expectedBankCode, account.BankCode);
        }


        [Theory]
        [InlineData("12/123")]
        [InlineData("12/12345")]
        [InlineData("12/abcd")]
        [InlineData("12/A100")]
        public void Input_Without4DigitBankCode_ThrowsFormatException(string invalidInput)
        {
            Assert.Throws<FormatException>(() => pattern.Parse(invalidInput).GetValueOrThrow());
        }

        [Fact]
        public void Format_WillFormatAccountNumberInStandardPattern()
        {
            var accountNumber = new AccountNumber(0, 0, "0100");
            Assert.Equal(accountNumber.ToString("S", null), pattern.Format(accountNumber));
        }

        [Fact]
        public void ApendBuilderCheckThatBuilderIsNotNull()
        {
            var accountNumber = new AccountNumber(0, 0, "0100");
            Assert.Throws<ArgumentNullException>(() => pattern.AppendFormat(accountNumber, null));
        }

        [Fact]
        public void ApendBuilderAppendsAccountNumberInStandardPattern()
        {
            var accountNumber = new AccountNumber(0, 0, "0100");
            var standardFormat = accountNumber.ToString("S", null);
            var builder = new StringBuilder("Hello, my account is ");
            builder = pattern.AppendFormat(accountNumber, builder);
            Assert.Equal("Hello, my account is 00/0100", builder.ToString());
        }
    }
}
