using System;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{

    /// <summary>
    /// Identifier number of a legal person in Czech Republic (also called IČO), either a company or a self-employed trader.
    /// </summary>
    /// <remarks>
    /// The number is 8 characters long and must contain only digits. Leading zeros are always included.
    /// Unfortunatelly, I was unable to find official definition, so I ended up with this: 
    /// <see cref="https://phpfashion.com/jak-overit-platne-ic-a-rodne-cislo"/>.
    /// </remarks>
    public class IdentificationNumber : IIdentifier
    {
        private const long numberLowerLimit = 0;
        private const long numberUpperLimit = 9999999;
        private const long checkDigitLowerLimit = 0;
        private const long checkDigitUpperLimit = 9;

        /// <summary>
        /// The input value of the number, might be valid, might be not.
        /// </summary>
        private readonly string input;

        /// <summary>
        /// The IČO number from first 7 digits.
        /// </summary>
        private long number;

        /// <summary>
        /// The IČO check digit.
        /// </summary>
        private int checkDigit;

        /// <summary>
        /// A constructor used to create a identifier number that was succesfully parsed (=has <see cref="HasStandardFormat"/> <c>true</c>).
        /// </summary>
        /// <param name="number">First seven digits of IČO, a number from <see cref="numberLowerLimit"/> to <see cref="upperLowerLimit"/>.</param>
        /// <param name="checkDigit">Check digit, doesn't have to be correct for the <paramref name="number"/>, but it must be from <see cref="checkDigitLowerLimit"/> to <see cref="checkDigitUpperLimit"/>.</param>
        /// <param name="input">The identifier number <paramref name="number"/> and <paramref name="checkDigit"/> were parsed from.</param>
        public IdentificationNumber(long number, int checkDigit, string input)
        {
            if (number < numberLowerLimit || number > numberUpperLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(number), $"Argument must be from {numberLowerLimit} to {numberUpperLimit}, but was {number}.");
            }

            if (checkDigit < checkDigitLowerLimit || checkDigit > checkDigitUpperLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(checkDigit), $"Argument must be from {checkDigitLowerLimit} to {checkDigitUpperLimit}, but was {checkDigit}.");
            }

            Input = input;
            HasStandardFormat = true;
            this.number = number;
            this.checkDigit = checkDigit;
        }

        /// <summary>
        /// Input value the identifier was created from, warts and all.
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// Does the identification number use standard 8 digit format?
        /// </summary>
        public bool HasStandardFormat { get; }

        /// <summary>
        /// Is the identifier valid according to the specification.
        /// </summary>
        public bool IsValid => HasStandardFormat && CheckDigit == ExpectedCheckDigit;

        /// <summary>
        /// Get a IČO number = umber constructed from first 7 digits (eight digit is check digit). Return null, if not in standard format.
        /// </summary>
        public long? Number => HasStandardFormat ? number : (long?)null;

        /// <summary>
        /// Get check digit of IČO (it is last, least significat digit). Return null, if not in standard format.
        /// </summary>
        public int? CheckDigit => HasStandardFormat ? checkDigit : (int?)null;

        /// <summary>
        /// Get expected check digit according to the checksum algorithm. Return null, if not in standard format.
        /// </summary>
        public int? ExpectedCheckDigit => HasStandardFormat ? CalculateExpectedCheckDigit(CalculateModulo(number)) : (int?)null;

        /// <summary>
        /// Get modulo from digits:
        /// (digit_1 * 8 + digit_2 * 7 + digit_3 * 6 + digit_4 * 5 + digit_5 * 4 + digit_6 * 3 + digit_7 * 2) mod 11
        /// </summary>
        /// <returns>Calculated modulo of the identification number.</returns>
        private int CalculateModulo(long number)
        {
            int weightedSum = 0;
            int weight = 2;
            while (number > 0)
            {
                var digit = (int)(number % 10);
                weightedSum = weightedSum + digit * weight;
                weight++;
                number = number / 10;
            }

            return weightedSum % 11;
        }

        private int CalculateExpectedCheckDigit(int modulo)
        {
            switch (modulo)
            {
                case 0:
                    return 1;
                case 1:
                    return 0;
                default:
                    return 11 - modulo;
            }
        }
    }
}
