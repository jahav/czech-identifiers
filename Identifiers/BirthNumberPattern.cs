using NodaTime.Text;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    /// <summary>
    /// Pattern for birth number.
    /// </summary>
    public sealed class BirthNumberPattern : IPattern<BirthNumber>
    {
        private const int yearDigit0 = 0;
        private const int yearDigit1 = 1;
        private const int monthDigit0 = 2;
        private const int monthDigit1 = 3;
        private const int dayDigit0 = 4;
        private const int dayDigit1 = 5;
        private const int sequenceDigit0 = 6;
        private const int sequenceDigit1 = 7;
        private const int sequenceDigit2 = 8;
        private const int checkDigit = 9;

        private readonly Regex regex;
        private readonly string format;

        /// <summary>
        /// A pattern for the birth number written as a 9 or 10 digit number (including leading zeros) with a slash between digit date part and sequence number.
        /// </summary>
        public static BirthNumberPattern StandardPattern { get; } = new BirthNumberPattern("^([0-9]{2})([0-9]{2})([0-9]{2})/([0-9]{3})([0-9])?$", "{0:00}{1:00}{2:00}{3:000}{4}");

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
        public static BirthNumberPattern NumberPattern { get; } = new BirthNumberPattern("^([0-9]{2})([0-9]{2})([0-9]{2})([0-9]{3})([0-9])?$", "{0:00}{1:00}{2:00}/{3:000}{4}");

        private BirthNumberPattern(string regexpPattern, string format)
        {
            regex = new Regex(regexpPattern);
            this.format = format;
        }

        public StringBuilder AppendFormat(BirthNumber value, StringBuilder builder)
        {
            return builder.Append(Format(value));
        }

        public string Format(BirthNumber value)
        {
            return string.Format(CultureInfo.InvariantCulture, format, value.yearPart, value.monthPart, value.dayPart, value.sequence, value.checkDigit);
        }

        public ParseResult<BirthNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var match = regex.Match(text);
            if (!match.Success)
            {
                return ParseResult<BirthNumber>.ForException(() => new FormatException($"Unable to parse '{text}'."));
            }

            var yearPart = ConvertGroupToNumber(match.Groups[1]);
            var monthPart = ConvertGroupToNumber(match.Groups[2]);
            var dayPart = ConvertGroupToNumber(match.Groups[3]);
            var sequence = ConvertGroupToNumber(match.Groups[4]);
            var checkDigitPart = match.Groups[5].Success ? ConvertGroupToNumber(match.Groups[5]) : (int?)null;

            var birthNumber = new BirthNumber(yearPart, monthPart, dayPart, sequence, checkDigitPart);
            return ParseResult<BirthNumber>.ForValue(birthNumber);
        }

        private int ConvertGroupToNumber(Group group)
        {
            var text = group.Captures[0].Value;
            var number = 0;
            for (int i = 0; i< text.Length; i++)
            {
                var digit = text[i] - '0';
                number = number * 10 + digit;
            }

            return number;
        }
    }
}
