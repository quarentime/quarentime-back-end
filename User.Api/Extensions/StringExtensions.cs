using System.Text.RegularExpressions;

namespace User.Api.Extensions
{
    public static class StringExtensions
    {
        public static string Initials(this string instance)
        {
            // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(instance, @"[\p{P}\p{S}\p{C}]+", "");

            // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            initials = Regex.Replace(initials, @"(\b\w)[\w]* ?", "$1");

            return initials.ToUpperInvariant();
        }
    }
}
