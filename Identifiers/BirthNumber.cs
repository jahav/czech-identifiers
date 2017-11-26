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
        private const int datePartLowerLimit = 0;
        private const int datePartUpperLimit = 99;
        private const int sequenceLowerLimit = 0;
        private const int sequenceUpperLimit = 999;
        private const int checkDigitLowerLimit = 0;
        private const int checkDigitUpperLimit = 9;

        private const int centuryYear1954 = 54;

        /// <summary>
        /// Birth numbers for women have added 50 for their month part, e.g. 455508/001 means a first woman born at 8 May 1945.
        /// <see cref="https://www.zakonyprolidi.cz/cs/2000-133/">Law number. 133/2000 § 13 (5)</see>
        /// </summary>
        internal const int WomanMonthShift = 50;

        /// <summary>
        /// When all sequential numbers for a day are assigned, it is possible to utilize another space by adding 20 to the month part.
        /// <see cref="https://www.zakonyprolidi.cz/cs/2000-133/">Law number. 133/2000 § 13 (5)</see>
        /// </summary>
        internal const int ExhaustMonthShift = 20;

        /// <summary>
        /// Every birth number after 1.1.1954 has a check digit, so it is 10 digits.
        /// </summary>
        /// <remarks><c>\d</c> character class includes other digits from other character set, so it is not used.</remarks>
        private readonly static Regex standardForm = new Regex("^[0-9]{9,10}$");

        private readonly int yearPart;
        private readonly int monthPart;
        private readonly int dayPart;
        private readonly int sequence;
        private readonly int? checkDigit;

        /// <summary>
        /// Create a new birth number in standard form.
        /// </summary>
        /// <param name="yearPart"></param>
        /// <param name="monthPart"></param>
        /// <param name="dayPart"></param>
        /// <param name="sequence"></param>
        /// <param name="checkDigit"></param>
        /// <param name="input"></param>
        public BirthNumber(int yearPart, int monthPart, int dayPart, int sequence, int? checkDigit, string input)
        {
            if (IsDatePartOutOfRange(yearPart))
            {
                throw new ArgumentOutOfRangeException(nameof(yearPart), $"Year part of birth number can be from {datePartLowerLimit} to {datePartUpperLimit}, but argument was {yearPart}.");
            }

            if (IsDatePartOutOfRange(monthPart))
            {
                throw new ArgumentOutOfRangeException(nameof(monthPart), $"Month part of birth number can be from {datePartLowerLimit} to {datePartUpperLimit}, but argument was {monthPart}.");
            }

            if (IsDatePartOutOfRange(dayPart))
            {
                throw new ArgumentOutOfRangeException(nameof(dayPart), $"Day part of birth number can be from {datePartLowerLimit} to {datePartUpperLimit}, but argument was {dayPart}.");
            }

            if (sequence < sequenceLowerLimit || sequence > sequenceUpperLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(sequence), $"Sequence of birth number can be from {sequenceLowerLimit} to {sequenceUpperLimit}, but argument was {sequence}.");
            }

            if (checkDigit.HasValue && (checkDigit < checkDigitLowerLimit || checkDigit > checkDigitUpperLimit))
            {
                throw new ArgumentOutOfRangeException(nameof(checkDigit), $"Check digit of birth number can either be empty or from {checkDigitLowerLimit} to {checkDigitUpperLimit}, but argument was {checkDigit}.");
            }

            HasStandardFormat = true;
            this.yearPart = yearPart;
            this.monthPart = monthPart;
            this.dayPart = dayPart;
            this.sequence = sequence;
            this.checkDigit = checkDigit;
            Input = input;
        }

        /// <summary>
        /// Input used to create the identifier.
        /// </summary>
        public string Input { get; }

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
                if (HasStandardFormat && IsDateValid)
                {
                    if (IsAfter1954)
                    {
                        return CalculateCheckDigit() == checkDigit;
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Return a year of birth, if the number has a standard form.
        /// </summary>
        public int? Year => HasStandardFormat ? CalculateYear() : (int?)null;

        /// <summary>
        /// Return a month of birth, if the number has a standard form. If the month part of birth 
        /// number is invalid, the returned month will be out of 1-12 range.
        /// </summary>
        public int? Month => HasStandardFormat ? CalculateMonth() : (int?)null;

        /// <summary>
        /// Return a day of birth, if the number has a standard form.
        /// </summary>
        public int? Day => HasStandardFormat ? dayPart : (int?)null;

        /// <summary>
        /// Get expected check digit, if the birth number has a standard form and is after year 1954.
        /// </summary>
        public int? ExpectedCheckDigit => HasStandardFormat && IsAfter1954 ? CalculateCheckDigit() : (int?)null;

        private bool IsAfter1954 => !(checkDigit is null);

        private bool IsDateValid
        {
            get
            {
                var month = CalculateMonth();
                if (month < 1 || month > 12 || dayPart < 1)
                {
                    return false;
                }

                if (dayPart > DateTime.DaysInMonth(CalculateYear(), month))
                {
                    return false;
                }

                return true;
            }
        }

        public int CalculateYear()
        {
            var yearInCentury = yearPart;
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
            var month = monthPart;
            var isWomen = monthPart > WomanMonthShift;
            if (isWomen)
            {
                month -= WomanMonthShift;
            }

            // law for exhaustion entered force at 2004
            var isExhaused = CalculateYear() > 2003 && month > ExhaustMonthShift;
            if (isExhaused)
            {
                month -= ExhaustMonthShift;
            }

            return month;
        }

        private int CalculateCheckDigit()
        {
            var number = ((yearPart * 1_00 + monthPart) * 1_00 + dayPart) * 1_000 + sequence;
            var modulo = number % 11;
            return modulo == 10 ? 0 : modulo;
        }

        private bool IsDatePartOutOfRange(int datePart)
        {
            return datePart < datePartLowerLimit || datePart > datePartUpperLimit;
        }
    }
}
