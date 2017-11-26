using System.Globalization;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    /// <summary>
    /// An account number used in Czech Republic, it consists from three parts:
    /// <ul>
    /// <li>Prefix (optional)</li>
    /// <li>Account number</li>
    /// <li>Bank code</li>
    /// </ul>
    /// and generally between prefix and account number is a dash, while between account number and bank code is a slash.
    /// <example>
    /// 19-123457/0710
    /// </example>
    /// </summary>
    /// <see cref="https://www.cnb.cz/miranda2/export/sites/www.cnb.cz/cs/platebni_styk/pravni_predpisy/download/vyhl_169_2011.pdf">Decree 169/2011</see>
    /// <see cref="https://www.cnb.cz/cs/platebni_styk/iban/iban_napoveda.html"/>
    public class AccountNumber : IIdentifier
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
        /// <param name="accountNumber">A string of the czech account number. Can be invalid or even null.</param>
        public AccountNumber(long prefix, long number, string bankCode, string input)
        {
            if (bankCode == null)
            {
                throw new System.ArgumentNullException(nameof(bankCode));
            }

            Input = input;
            HasStandardFormat = true;
            this.prefix = prefix;
            this.number = number;
            this.bankCode = bankCode;
        }

        /// <summary>
        /// The original input from which was the account number created.
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// Does the account number has a standard format.
        /// </summary>
        public bool HasStandardFormat { get; }

        /// <summary>
        /// Check if the account number is valid.
        /// </summary>
        /// <remarks>
        /// The account number is valid, if it has
        /// <ul>
        ///   <li>standard format.</li>
        ///   <li>Prefix weighted checksum is divisiable by 11.</li>
        ///   <li>Number weighted checksum is divisiable by 11.</li>
        ///   <li>Number has at least two non-zero digits.</li>
        /// </ul>
        /// </remarks>
        public bool IsValid => HasStandardFormat && PrefixChecksum % 11 == 0 && NumberChecksum % 11 == 0 && CalculateNonDigitCount(Number) >= 2;

        /// <summary>
        /// Prefix part of the the account, if the number has a standard form.
        /// </summary>
        public long? Prefix => HasStandardFormat ? prefix : (long?)null;

        /// <summary>
        /// The account number, if the number has a standard form. 
        /// </summary>
        public long? Number => HasStandardFormat ? number : (long?)null;

        /// <summary>
        /// Code of the bank where the account is, if the number has a standard form.
        /// </summary>
        public string BankCode => HasStandardFormat ? bankCode : null;

        /// <summary>
        /// Get a checksum for a prefix, if the number has a standard form.
        /// </summary>
        public int? PrefixChecksum => HasStandardFormat ? CheckSum(prefix) : (int?)null;

        /// <summary>
        /// Get a checksum for a prefix, if the number has a standard form.
        /// </summary>
        public int? NumberChecksum => HasStandardFormat ? CheckSum(number) : (int?)null;

        /// <summary>
        /// Get a number of digits that are not zero.
        /// </summary>
        /// <param name="part">Tested part</param>
        /// <returns>Number of digits in the <paramref name="part"/> that are not zero.</returns>
        private int CalculateNonDigitCount(long? part)
        {
            if (part == null) return 0;

            int count = 0;
            var value = part.Value;
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
    }
}
