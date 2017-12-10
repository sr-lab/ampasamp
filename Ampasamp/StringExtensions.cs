using System.Linq;

namespace Ampasamp
{
    /// <summary>
    /// Useful extensions to strings to check policy compliance.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Returns the number of lowercase characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        internal static int CountLowers(this string str)
        {
            return str.Count(char.IsLower);
        }

        /// <summary>
        /// Returns the number of uppercase characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        internal static int CountUppers(this string str)
        {
            return str.Count(char.IsUpper);
        }

        /// <summary>
        /// Returns the number of digits characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        internal static int CountDigits(this string str)
        {
            return str.Count(char.IsDigit);
        }

        /// <summary>
        /// Returns the number of non-alphanumeric characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        internal static int CountOthers(this string str)
        {
            return str.Length - str.Count(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Returns the number of character classes that appear in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        internal static int CountClasses(this string str)
        {
            var count = 0;
            count += str.CountLowers() > 0 ? 1 : 0;
            count += str.CountUppers() > 0 ? 1 : 0;
            count += str.CountDigits() > 0 ? 1 : 0;
            count += str.CountOthers() > 0 ? 1 : 0;

            return count;
        }

        /// <summary>
        /// Returns the number of words (letter sequences) that appear in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        internal static int CountWords(this string str)
        {
            var count = 0;
            var inWord = false;
            foreach (var chr in str)
            {
                if (char.IsLetter(chr))
                {
                    if (!inWord)
                    {
                        inWord = true;
                        count++;
                    }
                }
                else
                {
                    inWord = false;
                }
            }
            return count;
        }
    }
}
