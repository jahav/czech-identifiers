using System;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    public class BirthNumberParser : IIdentifierParser<string, BirthNumber>
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

        /// <summary>
        /// Every birth number after 1.1.1954 has a check digit, so it is 10 digits.
        /// </summary>
        /// <remarks><c>\d</c> character class includes other digits from other character set, so it is not used.</remarks>
        private readonly static Regex standardForm = new Regex("^[0-9]{9,10}$");

        /// <summary>
        /// Parse a birth number from the string in a standard form, e.g. YYMMDDSSS[C]?, where 
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
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public BirthNumber Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var hasStandardFormat = standardForm.IsMatch(input);
            if (!hasStandardFormat)
            {
                throw new FormatException($"Unable to parse number '{input}'. Expecting a number in a 'YYMMDDSSS[C]?' format.");
            }

            var birthNumber = input;
            var digits = new int[birthNumber.Length];
            for (int i = 0; i < birthNumber.Length; i++)
            {
                digits[i] = birthNumber[i] - '0';
            }

            var yearPart = digits[yearDigit0] * 10 + digits[yearDigit1];
            var monthPart = digits[monthDigit0] * 10 + digits[monthDigit1];
            var dayPart = digits[dayDigit0] * 10 + digits[dayDigit1];
            var sequence = digits[sequenceDigit0] * 100 + digits[sequenceDigit1] * 10 + digits[sequenceDigit2];

            return new BirthNumber(yearPart, monthPart, dayPart, sequence, (digits.Length > checkDigit) ? digits[checkDigit] : (int?)null, input);
        }



        private bool IsWoman(int monthPart) => monthPart > BirthNumber.WomanMonthShift;

        private bool InExhaustedRange(int monthPart, int year, bool isWoman)
        {
            if (isWoman)
            {
                monthPart -= BirthNumber.WomanMonthShift;
            }

            // law for exhaustion entered force at 2004
            return year > 2003 && monthPart > BirthNumber.ExhaustMonthShift;
        }

        private int CalculateMonth(int monthPart, int year, bool isWoman, bool isExhaustRange)
        {
            if (isWoman)
            {
                monthPart -= BirthNumber.WomanMonthShift;
            }

            // law for exhaustion entered force at 2004
            if (isExhaustRange)
            {
                monthPart -= BirthNumber.ExhaustMonthShift;
            }

            return monthPart;
        }
    }
}
