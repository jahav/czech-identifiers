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
        public static AccountNumberPattern StandardPattern = new AccountNumberPattern();

        private AccountNumberPattern()
        {
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

        public string Format(AccountNumber value)
        {
            throw new NotImplementedException();
        }

        public StringBuilder AppendFormat(AccountNumber value, StringBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
