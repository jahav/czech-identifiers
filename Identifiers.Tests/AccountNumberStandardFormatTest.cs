using Xunit;

namespace Identifiers.Czech.Tests
{
    public class AccountNumberStandardFormatTest
    {
        [Fact]
        public void DefaultFormatIsStandardFormat()
        {
            var accountNumber = new AccountNumber(0, 12, "0100");
            var defaultFormat = accountNumber.ToString(null, null);
            var standardFormat = accountNumber.ToString("S", null);
            Assert.Equal(standardFormat, defaultFormat);
        }

        [Fact]
        public void StandardFormatDoesntHavePrefixIfPrefixIsZero()
        {
            var accountNumber = new AccountNumber(0, 12, "0100");
            Assert.Equal("12/0100", accountNumber.ToString("S", null));
        }

        [Fact]
        public void StandardFormatHasPrefixIfPrefixIsntZero()
        {
            var accountNumber = new AccountNumber(15, 12, "0100");
            Assert.Equal("15-12/0100", accountNumber.ToString("S", null));
        }

        [Fact]
        public void StandardFormatHasTwoDigitsEvenIfNumberIsZero()
        {
            var accountNumber = new AccountNumber(0, 0, "0100");
            Assert.Equal("00/0100", accountNumber.ToString("S", null));
        }

        [Fact]
        public void StandardFormatIsCaseInsensitive()
        {
            var accountNumber = new AccountNumber(0, 12, "0100");
            Assert.Equal(accountNumber.ToString("S", null), accountNumber.ToString("s", null));
        }
    }
}
