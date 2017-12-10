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
        
        /// <summary>
        /// Serializes a collection of strings into one string.
        /// </summary>
        /// <param name="arr">The string collection to serialize.</param>
        /// <param name="json">Whether or not to return the result as JSON.</param>
        /// <returns></returns>
        static string Serialize(IEnumerable<string> arr, bool json)
        {
            return json ? JsonConvert.SerializeObject(arr) : String.Join(Environment.NewLine, arr);
        }

        static void Main(string[] args)
        {
            // Read, parse, randomize full password database.
            var rnd = new Random();
            var passwords = File.ReadAllText("pass_db.txt")
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(x => x.Trim())
                .OrderBy(x => rnd.Next()).ToArray();

            // Read in task.
            var task = JsonConvert.DeserializeObject<Task>(File.ReadAllText("policies.json"));
            Console.WriteLine("Executing task: " + task.Name);

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
                File.WriteAllText(policy.Name + ".txt", Serialize(filtered.Take(task.Sample), task.OutputJson));
            }
        }
    }
}
