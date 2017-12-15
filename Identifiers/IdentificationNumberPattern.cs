using System.Text.RegularExpressions;
using System;

namespace Identifiers.Czech
{
    /// <summary>
    /// <see cref="IPattern{TValue}">Pattern</see> for a <see cref="IdentificationNumber"/>.
    /// </summary>
    public sealed class IdentificationNumberPattern : IPattern<IdentificationNumber>
    {
        /// <summary>
        /// A regular expressin that defines standard form of the ICO.
        /// </summary>
        private static readonly Regex standardForm = new Regex("^([0-9]{7})([0-9])$");

        /// <summary>
        /// The standard pattern for 8 digit IČO (Identifikační Číslo Osoby).
        /// <list type="bullet">
        ///     <item>
        ///         <term>00007064</term>
        ///         <description>A number 706 with a check digit 4.</description>
        ///     </item>
        ///     <item>
        ///         <term>69663963</term>
        ///         <description>A number 6966396 with a check digit 3.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public static IdentificationNumberPattern StandardPattern { get; } = new IdentificationNumberPattern(); 

        private IdentificationNumberPattern()
        {
        }

        /// <inheritdoc />
        public ParseResult<IdentificationNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var match = standardForm.Match(text);
            if (!match.Success)
            {
                return ParseResult<IdentificationNumber>.ForException(new FormatException($"Unable to parse identifier number '{text}'. Expecting a text of 8 digits."));
            }

            var number = match.Groups[1].ConvertToLong();
            var checkDigit = match.Groups[2].ConvertToNumber();

            var identificationNumber = new IdentificationNumber(number, checkDigit);
            return ParseResult<IdentificationNumber>.ForValue(identificationNumber);
        }

        /// <summary>
        /// Format identification number according to a pattern.
        /// </summary>
        /// <param name="value">Identification number.</param>
        /// <returns>Identification number formatted to string.</returns>
        public string Format(IdentificationNumber value)
        {
            return value.ToString("S", null);
        }
    }
}
