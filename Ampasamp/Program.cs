using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ampasamp
{
    class Program
    {
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

            // Password length check.
            result = result && (str.Length >= policy.Length);

            // Word count check.
            result = result && (str.CountWords() >= policy.Words);

            return result;
        }

        static string SanitizeIdentifier(string s)
        {
            string ou = "";
            foreach (var c in s)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    ou += Char.ToLower(c);
                }
                else if (c == ' ' || c == '_')
                {
                    ou += "_";
                }
            }
            return ou;
        }

        /// <summary>
        /// Serializes a collection of strings into one string.
        /// </summary>
        /// <param name="arr">The string collection to serialize.</param>
        /// <param name="json">Whether or not to return the result as JSON.</param>
        /// <returns></returns>
        static string Serialize(Task task, Policy policy, IEnumerable<string> arr, string format)
        {
            switch (format)
            {
                case "coq":
                    var output = Properties.Resources.coq_template;
                    output = output.Replace("%NAME", SanitizeIdentifier(task.Name + "_" + policy.Name));
                    output = output.Replace("%PASSWORDS", "\"" + String.Join("\";" + Environment.NewLine + "  \"", arr) + "\"");
                    return output;
                case "json":
                    return JsonConvert.SerializeObject(arr);
                default:
                    return String.Join(Environment.NewLine, arr); // Plain is default.
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
                Console.WriteLine($"Error: database file '{taskFilename}' does not exist.");
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

            // Read, parse, randomize full password database.
            var rnd = new Random();
            var passwords = File.ReadAllText(databaseFilename)
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(x => x.Trim())
                .OrderBy(x => rnd.Next()).ToArray();

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
                Console.WriteLine("Filtering on policy " + policy.Name + "...");

                // Check if we have enough passwords to sample.
                var filtered = passwords.Where(x => Complies(x, policy));
                if (filtered.Count() < task.Sample)
                {
                    Console.WriteLine("Not enough compliant passwords in this database to sample " + task.Sample + " so sampling as much as possible (" + filtered.Count() + ")...");
                }

                // Output compliant passwords to file.
                File.WriteAllText(policy.Name + ".txt", Serialize(task, policy, filtered.Take(task.Sample), task.Output));
            }
        }

        static void Main(string[] args)
        {
            var sampleOptions = new SampleOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, sampleOptions))
            {
                Sample(sampleOptions.Database, sampleOptions.Task);
            }
        }
    }
}
