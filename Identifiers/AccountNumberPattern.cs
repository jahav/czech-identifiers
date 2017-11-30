using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    public class AccountNumberPattern : IPattern<AccountNumber>
    {
        /// <summary>
        /// Accoring to the CNB decree no.169/2011, the prefix and account number should be 
        /// "clearly separated", so it could possibly be something else than dash, but dash is used by everyone.
        /// Use parser for converions.
        /// </summary>
        private static Regex standardForm = new Regex("^(([0-9]{1,6})-|-?)([0-9]{2,10})/([0-9]{4}$)");

        /// <summary>
        /// Parse an account number, even with leading zeros. 
        /// </summary>
        public static AccountNumberPattern StandardPattern = new AccountNumberPattern("S");

        private readonly string format;

        private AccountNumberPattern(string format)
        {
            this.format = format;
        }

        public ParseResult<AccountNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Match match = standardForm.Match(text);
            if (!match.Success)
            {
                return ParseResult<AccountNumber>.ForException(() => new FormatException($"The account doesn't have expected format, it should be prefix-number/bank_code, but it {text}."));
            }

            long prefix = 0;
            bool prefixFound = match.Groups[2].Captures.Count == 1;
            if (prefixFound)
            {
                prefix = long.Parse(match.Groups[2].Captures[0].Value, NumberStyles.None, CultureInfo.InvariantCulture);
            }

            long number = long.Parse(match.Groups[3].Captures[0].Value, NumberStyles.None, CultureInfo.InvariantCulture);
            string bankCode = match.Groups[4].Captures[0].Value;

            return ParseResult<AccountNumber>.ForValue(new AccountNumber(prefix, number, bankCode));
        }

        /// <summary>
        /// Format account number in a specified format.
        /// </summary>
        /// <param name="value">Account number to format.</param>
        /// <returns>Formatted account number.</returns>
        public string Format(AccountNumber value)
        {
            return value.ToString(format, null);
        }

        /// <summary>
        /// Format the account number in standard pattern and append it to the <paramref name="builder"/>.
        /// </summary>
        /// <param name="value">Account number.</param>
        /// <param name="builder">Build to append formatted account number.</param>
        /// <returns>Builder with appended formatted <see cref="value">account number</see> in a <see cref="AccountNumberPattern.StandardPattern"/>.</returns>
        /// <exception cref="ArgumentNullException">Builder must not be null.</exception>
        public StringBuilder AppendFormat(AccountNumber value, StringBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AppendFormat(Format(value));
        }
    }
}
