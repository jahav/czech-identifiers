using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Identifiers.Czech
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
    /// <see cref="http://www.mvcr.cz/clanek/overovani-rodneho-cisla-331794.aspx"/>
    public class BirthNumber : IIdentifier
    {
        private const int centuryYear1954 = 54;
        private const int yearDigit0 = 0;
        private const int yearDigit1 = 1;
        private const int monthDigit0 = 2;
        private const int monthDigit1 = 3;
        private const int dayDigit0 = 4;
        private const int dayDigit1 = 5;
        private const int sequenceDigit0 = 6;
        private const int sequenceDigit1 = 7;
        private const int sequenceDigit2 = 8;
        private const int checkDigit = 9;

        /// <summary>
        /// Birth numbers for women have added 50 for their month part, e.g. 455508/001 means a first woman born at 8 May 1945.
        /// <see cref="https://www.zakonyprolidi.cz/cs/2000-133/">Law number. 133/2000 § 13 (5)</see>
        /// </summary>
        private const int womanMonthShift = 50;

        /// <summary>
        /// When all sequential numbers for a day are assigned, it is possible to utilize another space by adding 20 to the month part.
        /// <see cref="https://www.zakonyprolidi.cz/cs/2000-133/">Law number. 133/2000 § 13 (5)</see>
        /// </summary>
        private const int exhaustMonthShift = 20;

        /// <summary>
        /// Every birth number after 1.1.1954 has a check digit, so it is 10 digits.
        /// </summary>
        /// <remarks><c>\d</c> character class includes other digits from other character set, so it is not used.</remarks>
        private readonly static Regex standardForm = new Regex("^[0-9]{9,10}$");

        private readonly string input;

        /// <summary>
        /// Digits of the birth number. Only set if birth number is in standard form.
        /// </summary>
        private readonly int[] digits;
        private readonly int year;
        private readonly int month;
        private readonly int day;
        private readonly int expectedCheckDigit;
        private readonly bool isDatePartValid;

        /// <summary>
        /// Create a new instance of a <see cref="BirthNumber"/>.
        /// </summary>
        /// <param name="birthNumber">A string that will be used as a identification number. Can be null.</param>
        public BirthNumber(string birthNumber)
        {
            input = birthNumber;
            HasStandardFormat = birthNumber != null && standardForm.IsMatch(birthNumber);
            if (!HasStandardFormat)
            {
                return;
            }

            digits = new int[birthNumber.Length];
            for (int i = 0; i < birthNumber.Length; i++)
            {
                digits[i] = birthNumber[i] - '0';
            }

            year = CalculateYear();
            month = CalculateMonth();
            day = digits[dayDigit0] * 10 + digits[dayDigit1];
            isDatePartValid = CheckDateValidity();

            var number = 0;
            for (int i = yearDigit0; i <= sequenceDigit2; i++)
            {
                number = number * 10 + digits[i];
            }

            var modulo = number % 11;
            expectedCheckDigit = modulo == 10 ? 0 : modulo;
        }

        /// <summary>
        /// Does the input have a standard form?
        /// </summary>
        public bool HasStandardFormat { get; }

        /// <summary>
        /// Is the birth number standard and valid?
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (HasStandardFormat && isDatePartValid)
                {
                    if (IsAfter1954)
                    {
                        return expectedCheckDigit == digits[checkDigit];
                    }

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Return a year of birth, if the number has a standard form.
        /// </summary>
        public int? Year => HasStandardFormat ? year : (int?)null;

        /// <summary>
        /// Return a month of birth, if the number has a standard form.
        /// </summary>
        public int? Month => HasStandardFormat ? month : (int?)null;

        /// <summary>
        /// Return a day of birth, if the number has a standard form.
        /// </summary>
        public int? Day => HasStandardFormat ? day : (int?)null;

        /// <summary>
        /// Get expected check digit, if the birth number has a standard form and is after year 1954.
        /// </summary>
        public int? ExpectedCheckDigit => HasStandardFormat && IsAfter1954 ? expectedCheckDigit : (int?)null;

        private bool IsAfter1954 => input.Length == 10;

        public int CalculateYear()
        {
            var yearInCentury = digits[yearDigit0] * 10 + digits[yearDigit1];
            int year = yearInCentury;
            if (IsAfter1954)
            {
                if (yearInCentury < centuryYear1954)
                {
                    year += 2000;
                }
                else
                {
                    year += 1900;
                }
            }
            else
            {
                if (yearInCentury < centuryYear1954)
                {
                    year += 1900;
                }
                else
                {
                    year += 1800;
                }
            }

            return year;
        }

        private int CalculateMonth()
        {
            var month = digits[monthDigit0] * 10 + digits[monthDigit1];
            var isFemale = month > womanMonthShift;
            if (isFemale)
            {
                month -= 50;
            }

            // law for exhaustion entered force at 2004
            var isExhaused = year > 2003 && month > exhaustMonthShift;
            if (isExhaused)
            {
                month -= exhaustMonthShift;
            }

            return month;
        }

        private bool CheckDateValidity()
        {
            if (month < 1 || month > 12 || day < 1)
            {
                return false;
            }

            if (day > DateTime.DaysInMonth(year, month))
            {
                return false;
            }

            return true;
        }
    }
}
