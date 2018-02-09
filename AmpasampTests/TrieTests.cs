using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampasamp;

namespace AmpasampTests
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        public void TestInsert()
        {
            // Create empty trie.
            var subject = new Trie<int>();

            // Insert string.
            subject.Insert("hello", 0);

            // Trie should contain this string.
            Assert.IsTrue(subject.Contains("hello"));

            // Trie should not contain an unrelated string.
            Assert.IsFalse(subject.Contains("world"));

            // Trie should not contain a prefix of this string.
            Assert.IsFalse(subject.Contains("hell"));
        }
    }
}
