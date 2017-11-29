using System.Text.RegularExpressions;

namespace Identifiers.Czech
{
    internal static class ParseExtensions
    {
        public static int ConvertToNumber(this Group group)
        {
            var text = group.Captures[0].Value;
            var number = 0;
            for (int i = 0; i < text.Length; i++)
            {
                number = number * 10 + GetDigit(text, i);
            }

            return number;
        }

        public static long ConvertToLong(this Group group)
        {
            var text = group.Captures[0].Value;
            long number = 0;
            for (int i = 0; i < text.Length; i++)
            {
                number = number * 10 + GetDigit(text, i);
            }

            return number;
        }

        private static int GetDigit(string text, int index)
        {
            return text[index] - '0';
        }
    }
}
