using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Identifiers.Czech.Tests
{
    public class MrtdTd2Test
    {
        [Theory]
        [InlineData("I", true)]
        [InlineData("A", true)]
        [InlineData("C", true)]
        [InlineData("Z", false)]
        public void DocumentCode_OneLetterLong_IsValidWithIAC(string documentCode, bool isValid)
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(documentCode).Build();
            Assert.Equal(isValid, mrtd.IsValid);
        }

        [Fact]
        public void DocumentCode_EmptyString_IsNotValid()
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(string.Empty).Build();
            Assert.False(mrtd.IsValid);
        }

        [Theory]
        [InlineData("IRC")]
        [InlineData("ABCD")]
        public void DocumentCode_LongerThanTwoLetters_AreNeverValid(string documentCode)
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(documentCode).Build();
            Assert.False(mrtd.IsValid);
        }

        [Theory]
        [InlineData("IV")]
        [InlineData("AV")]
        public void DocumentCode_TwoLettersEndingWithV_IsNotValid(string documentCode)
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(documentCode).Build();
            Assert.False(mrtd.IsValid);
        }

        [Fact]
        public void DocumentCode_TwoLettersAC_IsNotValid()
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode("AC").Build();
            Assert.False(mrtd.IsValid);
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("AB")]
        [InlineData("C8")]
        public void DocumentCode_TwoLettersLongStartingWithIAC_IsValid(string documentCode)
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(documentCode).Build();
            Assert.True(mrtd.IsValid);
        }

        [Theory]
        [InlineData("BQ")]
        [InlineData("Z8")]
        public void DocumentCode_TwoLettersLongNotStartingWithIAC_IsNotValid(string documentCode)
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(documentCode).Build();
            Assert.False(mrtd.IsValid);
        }

        [Theory]
        [InlineData("Iš")]
        [InlineData("A$")]
        public void DocumentCode_SecondLetterNotDigitOrLetter_IsNotValid(string documentCode)
        {
            var mrtd = new MrtdTd2Builder().WithDocumentCode(documentCode).Build();
            Assert.False(mrtd.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("3v1")]
        [InlineData("čř")]
        public void IssuerCode_WithoutLetters_IsNotValid(string issuerCode)
        {
            var mrtd = new MrtdTd2Builder().WithIssuerCode(issuerCode).Build();
            Assert.False(mrtd.IsValid);
        }

        [Theory]
        [InlineData("ZZZ")]
        [InlineData("BC")]
        [InlineData("A")]
        public void IssuerCode_FromOneToThreeLetters_IsValid(string issuerCode)
        {
            var mrtd = new MrtdTd2Builder().WithIssuerCode(issuerCode).Build();
            Assert.True(mrtd.IsValid);
        }

        [Theory]
        [InlineData("CZECHIA")]
        [InlineData("ABCD")]
        public void IssuerCode_LongerThan4_IsNotValid(string issuerCode)
        {
            var mrtd = new MrtdTd2Builder().WithIssuerCode(issuerCode).Build();
            Assert.False(mrtd.IsValid);
        }
    }
}
