using System;

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
    public struct IdentificationNumber : IIdentifier
    {
        private const long numberLowerLimit = 0;
        private const long numberUpperLimit = 9999999;
        private const long checkDigitLowerLimit = 0;
        private const long checkDigitUpperLimit = 9;

        /// <summary>
        /// The IČO number from first 7 digits.
        /// </summary>
        private long number;

        /// <summary>
        /// The IČO check digit.
        /// </summary>
        private int checkDigit;

        /// <summary>
        /// A constructor used to create a identifier number.
        /// </summary>
        /// <param name="number">First seven digits of IČO, a number from <see cref="numberLowerLimit"/> to <see cref="upperLowerLimit"/>.</param>
        /// <param name="checkDigit">Check digit, doesn't have to be correct for the <paramref name="number"/>, but it must be from <see cref="checkDigitLowerLimit"/> to <see cref="checkDigitUpperLimit"/>.</param>
        /// 
        public IdentificationNumber(long number, int checkDigit)
        {
            if (number < numberLowerLimit || number > numberUpperLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(number), $"Argument must be from {numberLowerLimit} to {numberUpperLimit}, but was {number}.");
            }

            if (checkDigit < checkDigitLowerLimit || checkDigit > checkDigitUpperLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(checkDigit), $"Argument must be from {checkDigitLowerLimit} to {checkDigitUpperLimit}, but was {checkDigit}.");
            }

            this.number = number;
            this.checkDigit = checkDigit;
        }

        /// <summary>
        /// Is the identifier valid? I.e. its check digit is equal to expected check digit?
        /// </summary>
        public bool IsValid => CheckDigit == ExpectedCheckDigit;

        /// <summary>
        /// Get a IČO number = number constructed from first 7 digits (eight digit is check digit).
        /// </summary>
        public long Number => number;

        /// <summary>
        /// Get check digit of IČO (it is last, least significat digit).
        /// </summary>
        public int CheckDigit => checkDigit;

        /// <summary>
        /// Get expected check digit according to the checksum algorithm.
        /// </summary>
        public int ExpectedCheckDigit => CalculateExpectedCheckDigit(CalculateModulo(number));

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
