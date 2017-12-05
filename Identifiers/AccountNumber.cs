using System;
using System.Globalization;

namespace Identifiers.Czech
{
    /// <summary>
    /// A bank account number used in Czech Republic, it consists from three parts:
    /// <ul>
    /// <li>Prefix (optional)</li>
    /// <li>Account number</li>
    /// <li>Bank code</li>
    /// </ul>
    /// The leading zeros of prefix and number are not significant, so 00123 is same as 123.
    /// <para>
    /// <example>
    /// <c>19-123457/0710</c> is a bank account number with a prefix 19, account number 123457 and a bank code 0710.
    /// </example>
    /// </para>
    /// <para>
    /// <example>
    /// <c>0025478/0710</c> is a bank account number with a prefix 0, account number 25478 and a bank code 0710.
    /// </example>
    /// </para>
    /// The prefix and number should be visibly separated, but in practice everyone uses hypen. The number is 
    /// and generally between prefix and account number is a dash, while between account number and bank code is a slash.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The structure of account number is defined by ČNB at [decree 169/2011](https://www.cnb.cz/miranda2/export/sites/www.cnb.cz/cs/platebni_styk/pravni_predpisy/download/vyhl_169_2011.pdf).
    /// Each bank has a bank code, they are published by ČNB at [directory of payment system code](https://www.cnb.cz/en/payment_systems/accounts_bank_codes/index.html).
    /// ČNB has also published a <a href="https://www.cnb.cz/en/payment_systems/iban/iban_help.html">calculator</a> to convert account number to IBAN, the precise algorithm can be find at [Article 6 of decree 169/2011](http://www.cnb.cz/miranda2/export/sites/www.cnb.cz/en/legislation/decrees/decree_169_2011.pdf).
    /// </para>
    /// <para>
    /// The validity of the account number is checked by making sure that checksum of both prefix and number are fully divisible by 11.
    /// The checksum is calculated as a weighted sum of digits and its weights.
    /// The weights used for a calculation of a checksum are:
    /// <pre>
    /// |         |  A | B | C | D | E | F | G | H | I | J |
    /// | ------- | -- | - | - | - | - | - | - | - | - | - |
    /// | Digit   | 10 | 9 | 8 | 7 | 6 | 5 | 4 | 3 | 2 | 1 |  
    /// | weight  |  6 | 3 | 7 | 9 |10 | 5 | 8 | 4 | 2 | 1 |
    /// </pre>
    /// Where J is the rightest digit. You can find precise definition in decree 169/2011.
    /// <example>
    /// Number <c>7315789</c> has a checksum <c>7\*9 + 3\*10 + 1\*5 + 5\*8 + 7\*4 + 8\*2 +9\*1 = 191</c>. This number is not fully divisible by 11 (remainder is 4), so it would not be valid.
    /// </example>
    /// </para>
    /// </remarks>
    public struct AccountNumber : IIdentifier, IFormattable
    {
        /// <summary>
        /// Weights of the digits, from the rightmost to the leftmost (=index 0 is the rightmost one, index 9 is leftmost one).
        /// </summary>
        private static int[] weights = new int[] { 1, 2, 4, 8, 5, 10, 9, 7, 3, 6 };

        long prefix;
        long number;
        string bankCode;

        /// <summary>
        /// Create a new instance of a <see cref="AccountNumber"/>.
        /// </summary>
        /// <param name="prefix">The prefix number of the account.</param>
        /// <param name="number">The account number.</param>
        /// <param name="bankCode">A 4 digit bank code of the account, must not be null.</param>
        public AccountNumber(long prefix, long number, string bankCode)
        {
            if (bankCode == null)
            {
                throw new System.ArgumentNullException(nameof(bankCode));
            }

            this.prefix = prefix;
            this.number = number;
            this.bankCode = bankCode;
        }

        /// <summary>
        /// Check if the account number is valid.
        /// </summary>
        /// <remarks>
        /// The account number is valid, if it has
        /// <ul>
        ///   <li>Prefix weighted checksum is divisible by 11.</li>
        ///   <li>Number weighted checksum is divisible by 11.</li>
        ///   <li>Number has at least two non-zero digits.</li>
        /// </ul>
        /// </remarks>
        public bool IsValid => PrefixChecksum % 11 == 0 && NumberChecksum % 11 == 0 && CalculateNonDigitCount(Number) >= 2;

        /// <summary>
        /// Prefix part of the the account.
        /// </summary>
        public long Prefix => prefix;

        /// <summary>
        /// The number part of the account. 
        /// </summary>
        public long Number => number;

        /// <summary>
        /// Get code of the bank where the account is.
        /// </summary>
        public string BankCode => bankCode;

        /// <summary>
        /// Get a checksum for a prefix part of the account number.
        /// </summary>
        public int PrefixChecksum => CheckSum(prefix);

        /// <summary>
        /// Get a checksum for the number part of the account number.
        /// </summary>
        public int NumberChecksum => CheckSum(number);

        /// <summary>
        /// Get a number of digits that are not zero.
        /// </summary>
        /// <param name="part">Tested part</param>
        /// <returns>Number of digits in the <paramref name="part"/> that are not zero.</returns>
        private int CalculateNonDigitCount(long part)
        {
            int count = 0;
            var value = part;
            while (value > 0)
            {
                var digit = value % 10;
                if (digit != 0)
                {
                    count++;
                }

                value = value / 10;
            }

            return count;
        }

        /// <summary>
        /// Calculate a checksum of a part of the account.
        /// </summary>
        /// <param name="part">Part of the account, either prefix or account number.</param>
        /// <returns>Calculated checksum.</returns>
        private int CheckSum(long part)
        {
            var checksum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                int digit = (int)(part % 10);
                checksum += digit * weights[i];
                part = part / 10;
            }

            return checksum;
        }

        /// <summary>
        /// Format the account number according to the format.
        /// </summary>
        /// <remarks>
        /// The format 
        /// <list type="bullet">
        ///     <item>
        ///         <term>S</term>
        ///         <description>
        ///         <see cref="AccountNumberPattern.StandardPattern">Standard format</see> (numbers are printed without leadin zeros). If prefix is 0, it is omitted and no hypen is printed.
        ///         <example><c>25478/0710</c></example>
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>F</term>
        ///         <description>
        ///         Full format, both prefix and number will have full number of digits, including leading zeros.
        ///         <example><c>000000-0000025478/0710</c></example>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <param name="format">Format of the returned account. Can be null, in that case the <c>S</c> format will be used.</param>
        /// <param name="formatProvider">Not used.</param>
        /// <returns>Formatted account number.</returns>
        /// <exception cref="ArgumentException">When unknown <paramref name="format"/> is is passed.</exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                format = "S";
            }

            string prefixFormat;
            string remainderFormat;
            switch (format)
            {
                case "S":
                case "s":
                    prefixFormat = prefix != 0 ? "{0}-" : "";
                    remainderFormat = "{1:00}/{2}";
                    break;
                case "F":
                case "f":
                    prefixFormat = "{0:000000}-";
                    remainderFormat = "{1:0000000000}/{2}";
                    break;
                default:
                    throw new ArgumentException($"Unknown format {format}.", nameof(format));
            }

            return string.Format(CultureInfo.InvariantCulture, prefixFormat + remainderFormat, prefix, number, bankCode);
        }
    }
}
