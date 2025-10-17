using System.Globalization;
using System.Text;

namespace Common.Kernel.Extensions
{
    public static class StringExtension
    {

        /// <summary>
        /// Removes accents from a single string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>String with accents removed.</returns>
        public static string RemoveAccent(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var result = new string(normalizedString
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return result.Normalize(NormalizationForm.FormC);
        }
        public static string RemoveSpace(this string input)
        {
            return input.RemoveValue(" ");
        }

        public static string RemoveValue(this string input, string removed)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            var result = new string(input);
            result = result.Replace(removed, string.Empty);
            return result;
        }
    }
}
