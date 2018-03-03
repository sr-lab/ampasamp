namespace Ampasamp
{
    /// <summary>
    /// Represents a password policy.
    /// </summary>
    class Policy
    {
        /// <summary>
        /// Gets or sets the name of the policy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of passwords permitted under the policy.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of uppercase characters in passwords permitted under the policy.
        /// </summary>
        public int Uppers { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of digits in passwords permitted under the policy.
        /// </summary>
        public int Digits { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of lowercase characters in passwords permitted under the policy.
        /// </summary>
        public int Lowers { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of non-alphanumeric characters in passwords permitted under the policy.
        /// </summary>
        public int Others { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of character classes in passwords permitted under the policy.
        /// </summary>
        public int Classes { get; set; }
        
        /// <summary>
        /// Gets or sets the minimum number of words in passwords permitted under the policy.
        /// </summary>
        /// <remarks>A 'word' is defined to be a letter string delimited by non-letter characters.</remarks>
        public int Words { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of times a character in a password can be repeated.
        /// </summary>
        public int Repetitions { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum number of times a character in a password can vary from its predecessor by one code point.
        /// </summary>
        public int Consecutives { get; set; } = -1;

        /// <summary>
        /// Gets or sets the file path of the dictionary to use for the dictionary check.
        /// </summary>
        public string Dictionary { get; set; }
    }
}
