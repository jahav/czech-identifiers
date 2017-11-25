using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Identifiers
{
    /// <summary>
    /// A birth number is an identifier given to every born person in Czech Republic. It can be either 9 digits long (if assigned before 1954) or 10 digits long (assigned after 1954).
    /// 
    /// Birth number consists from:
    /// <ul>
    ///  <li>date of birth</li>
    ///  <li>sequence number</li>
    ///  <li>check digit (only after 1954)</li>
    /// </ul>
    /// </summary>
    public class BirthNumber
    {
        /// <summary>
        /// Every birth number after 1.1.1954 has a check digit, so it is 10 digits.
        /// </summary>
        /// <remarks><c>\d</c> character class includes other digits from other character set, so it is not used.</remarks>
        private static Regex standardForm = new Regex("^[0-9]{9,10}$");

        private string raw;

        /// <summary>
        /// Create a new instance of a <see cref="BirthNumber"/>.
        /// </summary>
        /// <param name="input">A string that will be used as a identification number. Can be null.</param>
        public BirthNumber(string input) {
            raw = input;
            HasStandardForm = input != null && standardForm.IsMatch(input);
        }

        public bool HasStandardForm { get; }
    }
}
