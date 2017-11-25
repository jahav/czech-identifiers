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
    }
}
