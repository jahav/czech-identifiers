using System;
using System.Collections.Generic;
using System.Text;

namespace Identifiers
{
    /// <summary>
    /// ABO account number format is an old standard for a banking account number for Czech Republic 
    /// from 1980. Back then there were only a few banks, so it fell from use, but it is still internally 
    /// used quite often in some banks (in many cases ABO is present, while normal account number or IBAN are missing).
    /// ABO is a 10 character string consisting from only digits.
    /// </summary>
    /// <remarks>ABO is an abberation of Automatic Banking Operations.</remarks>
    /// <see cref="https://www.penize.cz/bezne-ucty/15470-tajemstvi-cisla-uctu"/>
    internal class AboAccountNumber
    {
        private const int prefixDigit0 = 0;
        private const int prefixDigit1 = 1;
        private const int prefixDigit2 = 2;
        private const int prefixDigit3 = 3;
        private const int prefixDigit4 = 4;
        private const int checkPrefixDigit = 5;
        private const int accountDigit0 = 6;
        private const int accountDigit1 = 7;
        private const int accountDigit2 = 8;
        private const int accountDigit3 = 9;
        private const int accountDigit4 = 10;
        private const int organizationTypeDigit = 0;
        private const int branchDigit0 = 0;
        private const int branchDigit1 = 0;
        private const int bankDigit = 0;
    }
}
