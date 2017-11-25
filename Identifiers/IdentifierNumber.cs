using System.Text.RegularExpressions;

namespace Identifiers
{

    /// <summary>
    /// Identifier number of a legal person in Czech Republic (also called IČO), either a company or a self-employed trader.
    /// </summary>
    /// <remarks>
    /// The number is 8 characters long and must contain only digits. Leading zeros are always included.
    /// Unfortunatelly, I was unable to find official definition, so I ended up with this: 
    /// <see cref="https://phpfashion.com/jak-overit-platne-ic-a-rodne-cislo"/>.
    /// </remarks>
    public class IdentificationNumber
    {
        /// <summary>
        /// Length of the identification number.
        /// </summary>
        private const int length = 8;

        /// <summary>
        /// A regular expressin that defines standard form of the ICO.
        /// </summary>
        private static readonly Regex standardForm = new Regex("^[0-9]{8}$");

        /// <summary>
        /// The input value of the number, might be valid, might be not.
        /// </summary>
        private readonly string raw;

        /// <summary>
        /// Array of digits extracted from the <see cref="raw"/>, but only if it has <see cref="HasStandardFormat"/>.
        /// </summary>
        private readonly int[] digits;

        private readonly int modulo;

        private int expectedCheckDigit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">8 digit string in a standard form. Can be null.</param>
        public IdentificationNumber(string input)
        {
            raw = input;
            HasStandardFormat = raw != null && standardForm.IsMatch(raw);
            if (!HasStandardFormat)
            {
                return;
            }

            digits = new int[length];
            for (int i = 0; i < length; i++)
            {
                digits[i] = raw[i] - '0';
            }

            modulo = CalculateModulo();
            expectedCheckDigit = CalculateExpectedCheckDigit(modulo);
        }

        /// <summary>
        /// Does the identification number use standard 8 digit format?
        /// </summary>
        public bool HasStandardFormat { get; }

        /// <summary>
        /// Is the identifier valid according to the specification.
        /// </summary>
        public bool IsValid
        {
            get { return HasStandardFormat && CheckDigit == ExpectedCheckDigit; }
        }

        /// <summary>
        /// The actual check digit of the number. Return null, if not in standard format.
        /// </summary>
        public int? CheckDigit => HasStandardFormat ? digits[7] : (int?)null;

        /// <summary>
        /// The expected check digit according to the checksum algorithm. Return null, if not in standard format.
        /// </summary>
        public int? ExpectedCheckDigit => HasStandardFormat ? expectedCheckDigit : (int?)null;

        /// <summary>
        /// Get modulo from digits:
        /// (digit_1 * 8 + digit_2 * 7 + digit_3 * 6 + digit_4 * 5 + digit_5 * 4 + digit_6 * 3 + digit_7 * 2) mod 11
        /// </summary>
        /// <returns>Calculated modulo of the identification number.</returns>
        private int CalculateModulo()
        {
            var moduloSum = 0;
            for (int i = 0; i < length - 1; i++)
            {
                moduloSum += digits[i] * (8 - i);
            }

            return moduloSum % 11;
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
