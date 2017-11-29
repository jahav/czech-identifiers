using System;
using System.Text.RegularExpressions;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Identifiers.Tests")]
namespace Identifiers.Czech
{
    // TODO: make this internal, but attribute doesnt work
    /// <summary>
    /// A parser for standard format of the 
    /// </summary>
    [Obsolete("To be replaced by IdentifierNumberPattern")]
    public class IdentifierNumberParser : IIdentifierParser<string, IdentificationNumber>
    {
        /// <summary>
        /// A regular expressin that defines standard form of the ICO.
        /// </summary>
        private static readonly Regex standardForm = new Regex("^[0-9]{8}$");

        /// <summary>
        /// Parse an 8 digit IČO.
        /// </summary>
        /// <param name="input">Input to be parsed.</param>
        /// <returns>An IČO in a standard from.</returns>
        /// <inheritdoc/>
        public IdentificationNumber Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var hasStandardFormat = standardForm.IsMatch(input);
            if (!hasStandardFormat)
            {
                throw new FormatException($"Unable to parse number '{input}'. Expecting a number in a 'DDDDDDDC' format.");
            }

            var number = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                var digit = input[i] - '0';
                number = number * 10 + digit;
            }

            var checkDigit = input[input.Length - 1] - '0';
            return new IdentificationNumber(number, checkDigit);
        }
    }
}
