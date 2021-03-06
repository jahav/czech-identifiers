﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    /// <summary>
    /// A <see cref="IPattern{TValue}">pattern</see> for <see cref="AccountNumber"/>. It's not possible
    /// to create this class, you have to use provided <see cref="StandardPattern"/>.
    /// </summary>
    public sealed class AccountNumberPattern : IPattern<AccountNumber>
    {
        /// <summary>
        /// Accoring to the CNB decree no.169/2011, the prefix and account number should be 
        /// "clearly separated", so it could possibly be something else than dash, but dash is used by everyone.
        /// Use parser for converions.
        /// </summary>
        private static Regex standardForm = new Regex("^(([0-9]{1,6})-|-?)([0-9]{2,10})/([0-9]{4}$)");

        /// <summary>
        /// A pattern that doesn't use leading zeros and prefix can be missing if it is 0.
        /// 
        /// * ``19-5874/0300`` is correct format
        /// * ``5874/0300`` is correct format
        /// * ``019-5874/0300`` is incorrect format
        /// * ``000-5874/0300`` is incorrect format
        /// 
        /// The parsin is more lenient, it accepts both leading zeros and prefix consisting of only zeros.
        /// </summary>
        public static AccountNumberPattern StandardPattern = new AccountNumberPattern("S");

        private readonly string format;

        private AccountNumberPattern(string format)
        {
            this.format = format;
        }

        /// <inheritdoc />
        public ParseResult<AccountNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Match match = standardForm.Match(text);
            if (!match.Success)
            {
                return ParseResult<AccountNumber>.ForException(new FormatException($"The account doesn't have expected format, it should be prefix-number/bank_code, but it {text}."));
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
    }
}
