using Xunit;

namespace Identifiers.Czech.Tests
{
    public class AccountNumberFullFormatTest
    {
        [Fact]
        public void DefaultFormatIsntFullFormat()
        {
            var accountNumber = new AccountNumber(0, 12, "0100");
            var defaultFormat = accountNumber.ToString(null, null);
            var fullFormat = accountNumber.ToString("F", null);
            Assert.NotEqual(fullFormat, defaultFormat);
        }

        [Fact]
        public void FullFormatAlwaysHasFull6DigitsOfPrefix()
        {
            var accountNumber = new AccountNumber(0, 12, "0100");
            Assert.Equal("000000-0000000012/0100", accountNumber.ToString("F", null));
        }

        [Fact]
        public void FullFormatWorksWithMaximumNumber()
        {
            var accountNumber = new AccountNumber(999999, 9999999999L, "0100");
            Assert.Equal("999999-9999999999/0100", accountNumber.ToString("F", null));
        }

        [Fact]
        public void FullFormatIsCaseInsensitive()
        {
            var accountNumber = new AccountNumber(0, 12, "0100");
            Assert.Equal(accountNumber.ToString("F", null), accountNumber.ToString("f", null));
        }
    }
}
