using System;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Identifiers.Tests")]
namespace Identifiers.Czech
{
    /// <summary>
    /// <para>
    /// A birth number is an identifier given to every born person in Czech Republic. It can be either 
    /// 9 digits long (if assigned before 1954) or 10 digits long (assigned after 1954). It is used 
    /// extensively in many areas of life, especially in medicine and insurance.
    /// </para>
    /// <para>
    /// Birth number consists from:
    /// <ul>
    ///  <li>date of birth</li>
    ///  <li>sequence number</li>
    ///  <li>check digit (only after 1954)</li>
    /// </ul>
    /// <list type="bullet">
    ///     <item>
    ///         <term><example>675914/1488</example></term>
    ///         <description>A woman born 1967-09-14 with a sequence number 148 and check digit 8</description>
    ///     </item>
    ///     <item>
    ///         <term><example>350105/321</example></term>
    ///         <description>A man born 1935-01-05 with a sequence number 321</description>
    ///     </item>
    /// </list>
    /// </para>
    /// </summary>
    public struct BirthNumber : IIdentifier
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
        /// Create a new birth number in standard form.
        /// </summary>
        /// <param name="yearPart">A century part of the birth number, from 0-99 range.</param>
        /// <param name="monthPart">A month part of the birth number, from 0-99 range.</param>
        /// <param name="dayPart">A day part of the birth number, from 0-99 range.</param>
        /// <param name="sequence">A sequential assigned number in the day, from 0-999 range.</param>
        /// <param name="checkDigit">The last digit, null if date of birth is before 1954-01-01.</param>
        public BirthNumber(int yearPart, int monthPart, int dayPart, int sequence, int? checkDigit)
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

            YearPart = yearPart;
            MonthPart = monthPart;
            DayPart = dayPart;
            Sequence = sequence;
            CheckDigit = checkDigit;
        }

        /// <summary>
        /// Is the birth number valid? The birth number is valid, if its date is valid and 
        /// check digit is correct for birth numbers after 1954.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (IsDateValid)
                {
                    if (IsAfter1954)
                    {
                        return CalculateCheckDigit() == CheckDigit;
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Does the birth number belong to a woman? If true, the birth number belongs to a woman, if false, it belongs to a man.
        /// </summary>
        public bool BelongsToWoman => MonthPart > WomanMonthShift;

        /// <summary>
        /// Return a month of birth derived from birth number. If the month part of birth 
        /// number is invalid, the returned month will be out of 1-12 range.
        /// </summary>
        public int Month => CalculateMonth();

        /// <summary>
        /// Get date of birth. If the date of birth is not valid, return null.
        /// </summary>
        public DateTime? DateOfBirth
        {
            get
            {
                return CalculateDateOfBirth();
            }
        }

        /// <summary>
        /// Get expected check digit, if the birth number is after year 1954.
        /// </summary>
        public int? ExpectedCheckDigit => IsAfter1954 ? CalculateCheckDigit() : (int?)null;

        internal int YearPart { get; }
        internal int MonthPart { get; }
        internal int DayPart { get; }
        internal int Sequence { get; }
        internal int? CheckDigit { get; }

        private bool IsAfter1954 => !(CheckDigit is null);

        private bool IsDateValid => CalculateDateOfBirth() != null;

        private DateTime? CalculateDateOfBirth()
        {
            var month = CalculateMonth();
            if (month < 1 || month > 12 || DayPart < 1)
            {
                return null;
            }

            var year = CalculateYear();
            if (DayPart > DateTime.DaysInMonth(year, month))
            {
                return null;
            }

            return new DateTime(year, month, DayPart);
        }

        private int CalculateYear()
        {
            var yearInCentury = YearPart;
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
            var month = MonthPart;
            var isWomen = MonthPart > WomanMonthShift;
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
            var number = ((YearPart * 1_00 + MonthPart) * 1_00 + DayPart) * 1_000 + Sequence;
            var modulo = number % 11;
            return modulo == 10 ? 0 : modulo;
        }

        private static bool IsDatePartOutOfRange(int datePart)
        {
            return datePart < datePartLowerLimit || datePart > datePartUpperLimit;
        }

        /// <summary>
        /// Get formatted birth number.
        /// </summary>
        /// <param name="format"><c>Null</c>, <c>S</c> or <c>s</c> for <see cref="BirthNumberPattern.StandardPattern">standard format(slash between datepart and sequence)</see>. Use <c>N</c> or <c>n</c> for a <see cref="BirthNumberPattern.NumberPattern">number pattern (no slash, but include possible leadin zero).</see>.</param>
        /// <param name="formatProvider">Not used.</param>
        /// <returns>The formatted birth number.</returns>
        /// <exception cref="ArgumentException">If format doesn't have allowed values.</exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                format = "S";
            }

            switch (format)
            {
                case "N":
                case "n":
                    return string.Format("{0:00}{1:00}{2:00}{3:000}{4}", YearPart, MonthPart, DayPart, Sequence, CheckDigit);
                case "S":
                case "s":
                    return string.Format("{0:00}{1:00}{2:00}/{3:000}{4}", YearPart, MonthPart, DayPart, Sequence, CheckDigit);
                default:
                    throw new ArgumentException($"Format value {format} is not valid.", nameof(format));
            }
        }
    }
}
