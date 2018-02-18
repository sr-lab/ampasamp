using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ampasamp
{
    class Program
    {
        /// <summary>
        /// The dictionary cache.
        /// </summary>
        private static Dictionary<string, Trie<int>> DictionaryCache = new Dictionary<string, Trie<int>>();

        /// <summary>
        /// Reads a file as lines, returning it as an array of strings.
        /// </summary>
        /// <param name="filename">The filename of the file to read.</param>
        /// <returns></returns>
        private static IEnumerable<string> ReadFileAsLines(string filename)
        {
            return File.ReadAllText(filename)
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Loads the dictionary at a specified file path.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static Trie<int> LoadDictionary(string filepath)
        {
            // Cache if not cached already.
            if (!DictionaryCache.ContainsKey(filepath))
            {
                // Load into trie.
                var raw = ReadFileAsLines(filepath);

                // Remove comments and completely blank lines.
                raw = raw.Where(x => x != string.Empty && !x.StartsWith(("#!comment:")));

                // Build trie and cache.
                var trie = new Trie<int>();
                foreach (var entry in raw)
                {
                    trie.Insert(entry.ToLower(), 0);
                }
                DictionaryCache[filepath] = trie;
            }

            // Return dictionary.
            return DictionaryCache[filepath];
        }

        /// <summary>
        /// String all non-letter characters from a string.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <returns></returns>
        private static string StripNonLetters(string original)
        {
            var sb = new StringBuilder();
            foreach (var c in original)
            {
                if (char.IsLetter(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns true if a string complies with a policy, otherwise returns false.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="policy">The policy to check against.</param>
        /// <returns></returns>
        static bool Complies(string str, Policy policy)
        {
            var result = true;

            // Character class check.
            result = result && (str.CountUppers() >= policy.Uppers);
            result = result && (str.CountLowers() >= policy.Lowers);
            result = result && (str.CountDigits() >= policy.Digits);
            result = result && (str.CountOthers() >= policy.Others);
            result = result && (str.CountClasses() >= policy.Classes);

            // Dictionary check.
            if (policy.Dictionary != null && policy.Dictionary != string.Empty)
            {
                var term = StripNonLetters(str).ToLower();
                if (term != string.Empty)
                {
                    result = result && !LoadDictionary(policy.Dictionary).Contains(term);
                }
            }

            // Password length check.
            result = result && (str.Length >= policy.Length);

            // Word count check.
            result = result && (str.CountWords() >= policy.Words);

            return result;
        }

        /// <summary>
        /// Sanitizes an identifier for Coq.
        /// </summary>
        /// <param name="str">The identifier to sanitize.</param>
        /// <returns></returns>
        static string SanitizeIdentifier(string str)
        {
            string output = "";
            foreach (var chr in str)
            {
                if (char.IsLetterOrDigit(chr))
                {
                    output += chr;
                }
                else if (chr == ' ' || chr == '_')
                {
                    output += "_";
                }
            }
            return output;
        }

        /// <summary>
        /// Computes a suitable dictionary name from a task and policy.
        /// </summary>
        /// <param name="task">The task to use.</param>
        /// <param name="policy">The policy to use.</param>
        /// <returns></returns>
        static string ComputeName(Task task, Policy policy)
        {
            return SanitizeIdentifier($"{task.Name}_{policy.Name}_{task.Sample}");
        }

        /// <summary>
        /// Serializes a collection of strings into one string.
        /// </summary>
        /// <param name="task">The currently executing task (used for dictionary name generation).</param>
        /// <param name="policy">The current policy (used for dictionary name generation).</param>
        /// <param name="arr">The string collection to serialize.</param>
        /// <param name="json">Whether or not to return the result as JSON.</param>
        /// <returns></returns>
        static string Serialize(Task task, Policy policy, IEnumerable<string> arr, string format)
        {
            switch (format)
            {
                case "coq":
                    // Serialize into Coq template.
                    return Properties.Resources.coq_template 
                        .Replace("%NAME", ComputeName(task, policy))
                        .Replace("%PASSWORDS", $"\"{String.Join($"\";{Environment.NewLine}  \"", arr)}\"");
                case "json":
                    // Serialize as JSON.
                    return JsonConvert.SerializeObject(arr); 
                default:
                    // Plain is default.
                    return String.Join(Environment.NewLine, arr); 
            }
        }

        /// <summary>
        /// Gets the file extension for a specific output format.
        /// </summary>
        /// <param name="format">The output format string to get the extension for.</param>
        /// <returns></returns>
        static string GetExtension(string format)
        {
            switch (format)
            {
                case "coq":
                    return "v";
                case "json":
                    return "json";
                default:
                    return "txt";
            }
        }

        /// <summary>
        /// Executes a task file against a database.
        /// </summary>
        /// <param name="databaseFilename">The path of the database to execute against.</param>
        /// <param name="taskFilename">The task file to execute.</param>
        static void Sample(string databaseFilename, string taskFilename)
        {
            // Check files.
            if (!File.Exists(databaseFilename))
            {
                Console.WriteLine($"Error: database file '{databaseFilename}' does not exist.");
                return;
            }
            if (!File.Exists(taskFilename))
            {
                Console.WriteLine($"Error: task file '{taskFilename}' does not exist.");
                return;
            }

            // Read in task.
            var task = JsonConvert.DeserializeObject<Task>(File.ReadAllText(taskFilename));
            Console.WriteLine("Executing task: " + task.Name);

            // Read full passsword database.
            var passwords = ReadFileAsLines(databaseFilename);

            // Randomize if required.
            var rnd = new Random();
            if (task.RandomizeInitial)
            {
                Console.WriteLine("Performing initial randomization...");
                passwords = passwords.OrderBy(x => rnd.Next()).ToArray();
            }

            // Deduplicate if required.
            var withDuplicates = passwords.Count();
            if (task.Deduplicate)
            {
                Console.WriteLine("Deduplicating database...");
                passwords = passwords.Distinct();
                Console.WriteLine("Removed " + (withDuplicates - passwords.Count()) + " passwords.");
            }

            // Remove non-ASCII?
            if (task.CullNonAscii)
            {
                Console.WriteLine("Removing passwords containing non-ASCII characters...");
                var startCount = passwords.Count();
                passwords = passwords.Where(x => x.All(c => c >= 0 || c <= 127)).ToArray();
                Console.WriteLine("Removed " + (startCount - passwords.Count()) + " passwords.");
            }

            // Remove non-printable ASCII?
            if (task.CullNonPrintable)
            {
                Console.WriteLine("Removing passwords containing non-printable ASCII characters...");
                var startCount = passwords.Count();
                passwords = passwords.Where(x => x.All(c => c >= 32 || c <= 126)).ToArray();
                Console.WriteLine("Removed " + (startCount - passwords.Count()) + " passwords.");
            }

            // Filter on policies.
            foreach (var policy in task.Policies)
            {
                // Randomize on each sample if required.
                if (task.RandomizeEachSample)
                {
                    Console.WriteLine("Randomizing...");
                    passwords = passwords.OrderBy(x => rnd.Next()).ToArray();
                }

                Console.WriteLine("Filtering on policy " + policy.Name + "...");

                // Check if we have enough passwords to sample.
                var filtered = passwords.Where(x => Complies(x, policy));
                if (filtered.Count() < task.Sample)
                {
                    Console.WriteLine($"Not enough compliant passwords in this database to sample {task.Sample}"
                        + $" so sampling as much as possible ({filtered.Count()})...");
                }

                // Output compliant passwords to file.
                File.WriteAllText($"{ComputeName(task, policy)}.{GetExtension(task.Output)}", 
                    Serialize(task, policy, filtered.Take(task.Sample), task.Output));
            }
        }

        static void Main(string[] args)
        {
            // Parse arguments.
            var sampleOptions = new SampleOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, sampleOptions))
            {
                Sample(sampleOptions.Database, sampleOptions.Task);
            }
        }
    }
}
