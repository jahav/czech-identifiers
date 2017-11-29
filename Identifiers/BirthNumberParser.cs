using System;

namespace Identifiers.Czech
{
    [Obsolete("Replaced by BirthNumberPattern")]
    public class BirthNumberParser : IIdentifierParser<string, BirthNumber>
    {
        public BirthNumber Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var parseResult = BirthNumberPattern.NumberPattern.Parse(input);
            if (!parseResult.Success)
            {
                throw parseResult.Exception;
            }

            return parseResult.Value;
        }
    }
}
