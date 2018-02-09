using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampasamp
{
    public class Trie<T>
    {
        public char Prefix { get; private set; }

        public bool Terminal { get; private set; }

        public T Data { get; private set; }

        public List<Trie<T>> Children { get; private set; }

        public Trie(char prefix, bool terminal)
        {
            Prefix = prefix;
            Terminal = terminal;
            Children = new List<Trie<T>>();
        }

        public Trie()
        {
            Terminal = true;
            Children = new List<Trie<T>>();
        }

        public Trie(char prefix, bool terminal, T data) : this(prefix, terminal)
        {
            Data = data;
        }
        
        private Trie<T> GetChild(char prefix)
        {
            return Children.FirstOrDefault(c => c.Prefix == prefix);
        }

        public void Insert(string term, T data)
        {
            if (term == "")
            {
                Data = data;
                Terminal = true;
                return;
            }

            var child = GetChild(term[0]);
            if (child == null)
            {
                child = new Trie<T>(term[0], false);
                Children.Add(child);
            }

            child.Insert(term.Substring(1), data);
        }

        /// <summary>
        /// Returns true if this trie contains the given term, otherwise returns false.
        /// </summary>
        /// <param name="term">The search term.</param>
        /// <returns></returns>
        public bool Contains(string term)
        {
            // If we've reached the end of the term, we need to be on a terminal node.
            if (term == "")
            {
                return Terminal;
            }

            // If we can't go deeper into tree, return false.
            var child = GetChild(term[0]);
            if (child == null)
            {
                return false;
            }

            // Call down to child node.
            return child.Contains(term.Substring(1));
        }
    }
}
