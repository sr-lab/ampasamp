﻿namespace Ampasamp
{
    /// <summary>
    /// Represents a sampling task.
    /// </summary>
    class Task
    {
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sample size for the task.
        /// </summary>
        public int Sample { get; set; }

        /// <summary>
        /// Gets or sets the format to use in output dictionaries.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets whether or not to remove passwords containing non-ASCII characters.
        /// </summary>
        public bool CullNonAscii { get; set; }

        /// <summary>
        /// Gets or sets whether or not to remove passwords containing non-printable ASCII characters.
        /// </summary>
        public bool CullNonPrintable { get; set; }

        /// <summary>
        /// The set of policies to be used to sample data for this task.
        /// </summary>
        public Policy[] Policies { get; set; }
    }
}