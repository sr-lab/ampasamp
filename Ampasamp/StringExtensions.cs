using System;
using System.Linq;

namespace Ampasamp
{
    /// <summary>
    /// Useful extensions to strings to check policy compliance.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the number of lowercase characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountLowers(this string str)
        {
            return str.Count(char.IsLower);
        }

        /// <summary>
        /// Returns the number of uppercase characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountUppers(this string str)
        {
            return str.Count(char.IsUpper);
        }

        /// <summary>
        /// Returns the number of digits characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountDigits(this string str)
        {
            return str.Count(char.IsDigit);
        }

        /// <summary>
        /// Returns the number of non-alphanumeric characters in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountOthers(this string str)
        {
            return str.Length - str.Count(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Returns the number of character classes that appear in a string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountClasses(this string str)
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
        public static int CountWords(this string str)
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

        /// <summary>
        /// Returns the length of the longest repeated character run in the string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountRepetitions(this string str)
        {
            // Zero for an empty string.
            if (str.Length == 0)
            {
                return 0;
            }

            // Keep maximum count.
            var maxCount = 0;

            // Count character repetitions.
            var count = 0;
            var previous = str[0];
            for (var i = 1; i < str.Length; i++)
            {
                if (str[i] == previous)
                {
                    count++;
                }
                else
                {
                    maxCount = Math.Max(maxCount, count);
                    count = 0;
                }
                previous = str[i];
            }
            maxCount = Math.Max(maxCount, count); // Don't forget this at end of string.

            return maxCount;
        }

        /// <summary>
        /// Returns the length of the longest consecutive character run in the string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns></returns>
        public static int CountConsecutives(this string str)
        {
            // Zero for an empty string.
            if (str.Length == 0)
            {
                return 0;
            }

            // Keep maximum count.
            var maxCount = 0;

            // Count character repetitions.
            var count = 0;
            var previous = str[0];
            for (var i = 1; i < str.Length; i++)
            {
                if (str[i] + 1 == previous || str[i] - 1 == previous)
                {
                    count++;
                }
                else
                {
                    maxCount = Math.Max(maxCount, count);
                    count = 0;
                }
                previous = str[i];
            }
            maxCount = Math.Max(maxCount, count); // Don't forget this at end of string.

            return maxCount;
        }
    }
}
