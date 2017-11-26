using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    internal class BirthNumberParser : IIdentifierParser<string, BirthNumber>
    {
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
            throw new NotImplementedException();
        }
    }
}
