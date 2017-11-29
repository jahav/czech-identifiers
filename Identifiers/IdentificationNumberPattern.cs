using System.Text;
using NodaTime.Text;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace Identifiers.Czech
{
    public sealed class IdentificationNumberPattern : IPattern<IdentificationNumber>
    {
        /// <summary>
        /// A regular expressin that defines standard form of the ICO.
        /// </summary>
        private static readonly Regex standardForm = new Regex("^([0-9]{7})([0-9])$");

        /// <summary>
        /// Pattern for 8 digit IČO (Identifikační Číslo Osoby).
        /// </summary>
        /// <example><c>00006947</c></example>
        public static IdentificationNumberPattern StandardPattern { get; } = new IdentificationNumberPattern(); 

        private IdentificationNumberPattern()
        {
        }

        public ParseResult<IdentificationNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var match = standardForm.Match(text);
            if (!match.Success)
            {
                return ParseResult<IdentificationNumber>.ForException(() => throw new FormatException($"Unable to parse identifier number '{text}'. Expecting a text of 8 digits."));
            }

            var number = match.Groups[1].ConvertToLong();
            var checkDigit = match.Groups[2].ConvertToNumber();

            var identificationNumber = new IdentificationNumber(number, checkDigit);
            return ParseResult<IdentificationNumber>.ForValue(identificationNumber);
        }

        // TODO: What about null value? Exception  or empty string?
        public string Format(IdentificationNumber value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:00000000}", value);
        }

        // TODO: What about null value? Exception  or empty string?
        public StringBuilder AppendFormat(IdentificationNumber value, StringBuilder builder)
        {
            return builder.AppendFormat(Format(value));
        }
    }
}
