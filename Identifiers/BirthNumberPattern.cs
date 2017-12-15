using System;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    /// <summary>
    /// <see cref="IPattern{TValue}">Pattern</see> for a <see cref="BirthNumber"/>.
    /// </summary>
    public sealed class BirthNumberPattern : IPattern<BirthNumber>
    {
        private readonly Regex regex;
        private readonly string format;

        /// <summary>
        /// A pattern for the birth number written as a 9 or 10 digit number (including leading zeros) with a slash between digit date part and sequence number. The parsed number must include extra leading zeros and slash.
        /// <example>
        /// <list type="bullet">
        ///     <item>
        ///         <term>081102/789</term>
        ///         <description>A birth number of a person born at 1908-11-02.</description>
        ///     </item>
        ///     <item>
        ///         <term>795322/9944</term>
        ///         <description>A birth number of a person born at 1979-03-22</description>
        ///     </item>
        /// </list>
        /// </example>
        /// </summary>
        public static BirthNumberPattern StandardPattern { get; } = new BirthNumberPattern("^([0-9]{2})([0-9]{2})([0-9]{2})/([0-9]{3})([0-9])?$", "N");

        /// <summary>
        /// A pattern for the birth number written as a 9 or 10 digit number including leading zeros.
        /// </summary>
        /// <remarks>
        /// Parses a birth number from the string in a standard form, e.g. YYMMDDSSS[C]?, where 
        /// <list type="bullet">
        ///     <item>
        ///         <term>YY</term>
        ///         <description>a year in the century.</description>
        ///     </item>
        ///     <item>
        ///         <term>MM</term>
        ///         <description>a month from 1 to 12. If a person is a female, add 50 to the number. It is possible fro month to be shifted by another 20 (after year 2003), if all birth numbers have been exhaused.</description>
        ///     </item>
        ///     <item>
        ///         <term>DD</term>
        ///         <description>a day of a month, from  1 to the number of days in month.</description>
        ///     </item>
        ///     <item>
        ///         <term>SSS</term>
        ///         <description>Sequential number od assigned number, from 000 to 999.</description>
        ///     </item>
        ///     <item>
        ///         <term>C</term>
        ///         <description>Optional check digit, only used for birth numbers assigned at/after 1.1.1954.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static BirthNumberPattern NumberPattern { get; } = new BirthNumberPattern("^([0-9]{2})([0-9]{2})([0-9]{2})([0-9]{3})([0-9])?$", "S");

        private BirthNumberPattern(string regexpPattern, string format)
        {
            regex = new Regex(regexpPattern);
            this.format = format;
        }

        /// <inheritdoc />
        public string Format(BirthNumber value)
        {
            return value.ToString(format, null);
        }

        /// <inheritdoc />
        public ParseResult<BirthNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var match = regex.Match(text);
            if (!match.Success)
            {
                return ParseResult<BirthNumber>.ForException(new FormatException($"Unable to parse '{text}'."));
            }

            var yearPart = match.Groups[1].ConvertToNumber();
            var monthPart = match.Groups[2].ConvertToNumber();
            var dayPart = match.Groups[3].ConvertToNumber();
            var sequence = match.Groups[4].ConvertToNumber();
            var checkDigitPart = match.Groups[5].Success ? match.Groups[5].ConvertToNumber() : (int?)null;

            var birthNumber = new BirthNumber(yearPart, monthPart, dayPart, sequence, checkDigitPart);
            return ParseResult<BirthNumber>.ForValue(birthNumber);
        }
    }
}
