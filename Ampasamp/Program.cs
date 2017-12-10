﻿using Newtonsoft.Json;
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
                if (Char.IsLetterOrDigit(chr))
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
