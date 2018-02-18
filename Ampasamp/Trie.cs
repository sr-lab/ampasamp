using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampasamp
{
    /// <summary>
    /// Represents a trie (search tree).
    /// </summary>
    /// <typeparam name="T">The type of data to store as values in the trie.</typeparam>
    public class Trie<T>
    {
        /// <summary>
        /// Gets the prefix character associated with this trie.
        /// </summary>
        public char Prefix { get; private set; }

        /// <summary>
        /// Gets whether or not this tree represents a terminal node.
        /// </summary>
        public bool Terminal { get; private set; }

        /// <summary>
        /// Gets the data associated with this trie.
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// Gets the list of child nodes for this trie.
        /// </summary>
        public List<Trie<T>> Children { get; private set; }

        /// <summary>
        /// Initialises a new instance of a trie.
        /// </summary>
        public Trie()
        {
            Terminal = true;
            Children = new List<Trie<T>>();
        }

        /// <summary>
        /// Initializes a new instance of a trie.
        /// </summary>
        /// <param name="prefix">The prefix character to associate with the trie.</param>
        /// <param name="terminal">Whether or not this trie represents a terminal node.</param>
        public Trie(char prefix, bool terminal) : this()
        {
            Prefix = prefix;
            Terminal = terminal;
        }

        /// <summary>
        /// Initializes a new instance of a trie.
        /// </summary>
        /// <param name="prefix">The prefix character to associate with the trie.</param>
        /// <param name="terminal">Whether or not this trie represents a terminal node.</param>
        /// <param name="data">The data to associate with this trie.</param>
        public Trie(char prefix, bool terminal, T data) : this(prefix, terminal)
        {
            Data = data;
        }
        
        /// <summary>
        /// Gets the child of this trie with the specified prefix character.
        /// </summary>
        /// <param name="prefix">The prefix character to search for.</param>
        /// <returns></returns>
        private Trie<T> GetChild(char prefix)
        {
            return Children.FirstOrDefault(c => c.Prefix == prefix);
        }

        /// <summary>
        /// Inserts a piece of data into the trie associated witht the specified key.
        /// </summary>
        /// <param name="key">The key to map the data against.</param>
        /// <param name="data">The data to associate with the key.</param>
        public void Insert(string key, T data)
        {
            // If term is empty, we're at a terminal node, assign data.
            if (key == "")
            {
                Data = data;
                Terminal = true;
                return;
            }

            // Add new node if necessary.
            var child = GetChild(key[0]);
            if (child == null)
            {
                child = new Trie<T>(key[0], false);
                Children.Add(child);
            }

            // Move deeper into trie, remove first character from term.
            child.Insert(key.Substring(1), data);
        }

        /// <summary>
        /// Gets the value associated with the given key in the trie.
        /// </summary>
        /// <param name="key">The key to get the value for.</param>
        /// <returns></returns>
        public T Get(string key)
        {
            // If we've reached the end of the key, we need to be on a terminal node.
            if (key == "")
            {
                if (Terminal)
                {
                    return Data;
                }
                else
                {
                    // Not found.
                    throw new KeyNotFoundException();
                }
            }

            // If we can't go deeper into tree, throw exception.
            var child = GetChild(key[0]);
            if (child == null)
            {
                // Not found.
                throw new KeyNotFoundException();
            }

            // Call down to child node.
            return child.Get(key.Substring(1));
        }

        /// <summary>
        /// Returns true if this trie contains the given key, otherwise returns false.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            // If we've reached the end of the key, we need to be on a terminal node.
            if (key == "")
            {
                return Terminal;
            }

            // If we can't go deeper into tree, return false.
            var child = GetChild(key[0]);
            if (child == null)
            {
                return false;
            }

            // Call down to child node.
            return child.Contains(key.Substring(1));
        }
    }
}
