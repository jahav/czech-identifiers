using System;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class AccountNumberParserTest
    {
        private AccountNumberParser parser = new AccountNumberParser();

        [Fact]
        public void ParseDoesntAcceptNull()
        {
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Theory]
        [InlineData("-12/0100", 0)]
        [InlineData("00123-12/0100", 123)]
        [InlineData("123456-12/0100", 123456)]
        public void Input_WithPrefix0To6Digits_IsParsed(string accountNumber, long expectedPrefix)
        {
            Assert.Equal(expectedPrefix, parser.Parse(accountNumber).Prefix);
        }

        [Theory]
        [InlineData("aa-12/0100")]
        [InlineData("0000123-12/0100")]
        [InlineData("abc-12/0100")]
        public void Input_WithoutPrefix0To6Digits_ThrowFormatException(string accountNumber)
        {
            Assert.Throws<FormatException>(() => parser.Parse(accountNumber));
        }

        [Theory]
        [InlineData("12/0100", 12)]
        [InlineData("00123/0100", 123)]
        [InlineData("1234567890/0100", 1234567890L)]
        public void Input_WithNumber2To10Digits_IsParsed(string accountNumber, long expectedNumber)
        {
            Assert.Equal(expectedNumber, parser.Parse(accountNumber).Number);
        }

        [Theory]
        [InlineData("1/0100")]
        [InlineData("0/0100")]
        [InlineData("12345678901/0100")]
        [InlineData("abd/0100")]
        public void Input_WithoutNumber2To10Digits_ThrowsFormatException(string accountNumber)
        {
            Assert.Throws<FormatException>(() => parser.Parse(accountNumber));
        }

        [Theory]
        [InlineData("0012/0300", 12, "0300")]
        [InlineData("1234567890/6200", 1234567890, "6200")]
        [InlineData("0000/6200", 0, "6200")]
        public void CanAcceptAccountWithoutPrefix(string accountNumber, int expectedNumber, string expectedBankCode)
        {
            var account = parser.Parse(accountNumber);
            Assert.Equal(expectedNumber, account.Number);
            Assert.Equal(expectedBankCode, account.BankCode);
        }

        [Theory]
        [InlineData("17-0012/0300", 17, 12, "0300")]
        [InlineData("000-1234567890/6200", 0, 1234567890, "6200")]
        public void CanAcceptAccountWithPrefix(string accountNumber, int expectedPrefix, int expectedNumber, string expectedBankCode)
        {
            var account = parser.Parse(accountNumber);
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
            Assert.Throws<FormatException>(() => parser.Parse(invalidInput));
        }


    }
}
