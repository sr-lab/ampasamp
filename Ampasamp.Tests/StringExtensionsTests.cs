using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampasamp.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void TestCountLowers()
        {
            // Only lowercase letters should be counted.
            Assert.AreEqual(0, "ABACUS".CountLowers());
            Assert.AreEqual(6, "abacus".CountLowers());
            Assert.AreEqual(2, "abACUS".CountLowers());
            Assert.AreEqual(3, "abaCU$".CountLowers());
            Assert.AreEqual(2, "ab4CU$".CountLowers());
        }

        [TestMethod]
        public void TestCountUppers()
        {
            // Only uppercase letters should be counted.
            Assert.AreEqual(6, "ABACUS".CountUppers());
            Assert.AreEqual(0, "abacus".CountUppers());
            Assert.AreEqual(4, "abACUS".CountUppers());
            Assert.AreEqual(2, "abaCU$".CountUppers());
            Assert.AreEqual(2, "ab4CU$".CountUppers());
        }

        [TestMethod]
        public void TestCountDigits()
        {
            // Only digits should be counted.
            Assert.AreEqual(0, "ABACUS".CountDigits());
            Assert.AreEqual(0, "abacus".CountDigits());
            Assert.AreEqual(0, "abACUS".CountDigits());
            Assert.AreEqual(0, "abaCU$".CountDigits());
            Assert.AreEqual(1, "ab4CU$".CountDigits());
        }

        [TestMethod]
        public void TestCountOthers()
        {
            // Only digits should be counted.
            Assert.AreEqual(0, "ABACUS".CountOthers());
            Assert.AreEqual(0, "abacus".CountOthers());
            Assert.AreEqual(0, "abACUS".CountOthers());
            Assert.AreEqual(1, "abaCU$".CountOthers());
            Assert.AreEqual(1, "ab4CU$".CountOthers());
        }

        [TestMethod]
        public void TestCountClasses()
        {
            // There are 0 classes in the empty string.
            Assert.AreEqual(0, "".CountClasses());

            // There is 1 class in each of these 1-character strings.
            Assert.AreEqual(1, "1".CountClasses());
            Assert.AreEqual(1, "a".CountClasses());
            Assert.AreEqual(1, "A".CountClasses());
            Assert.AreEqual(1, "$".CountClasses());

            // This string contains all classes (a total of 4).
            Assert.AreEqual(4, "1aA$".CountClasses());

            // This string contains multiple instances of each class, but class count should still be 4.
            Assert.AreEqual(4, "123abcABC$%^".CountClasses());
        }

        [TestMethod]
        public void TestCountWords()
        {
            // There are 0 words in the empty string.
            Assert.AreEqual(0, "".CountWords());

            // There are 0 words in a string consisting only of non-letters.
            Assert.AreEqual(0, "1234@#$123".CountWords());

            // There is 1 word in a string consisting only of letters of the same case.
            Assert.AreEqual(1, "abacus".CountWords());
            Assert.AreEqual(1, "ABACUS".CountWords());

            // There is 1 word in a string consisting only of mixed-case letters.
            Assert.AreEqual(1, "aBAcus".CountWords());

            // There is 1 word in a string with non-letters at the beginning, end or both.
            Assert.AreEqual(1, "!abacus".CountWords());
            Assert.AreEqual(1, "abacus!".CountWords());
            Assert.AreEqual(1, "!abacus!".CountWords());

            // There are multiple words in strings separated by non-letter characters.
            Assert.AreEqual(2, "abacus!aardvark".CountWords());
            Assert.AreEqual(2, "abacus!aardvark!".CountWords());
            Assert.AreEqual(3, "abacus!aardvark8amulet".CountWords());
            Assert.AreEqual(3, "!abacus!aardvark8amulet".CountWords());
        }

        [TestMethod]
        public void TestGetRepetitions()
        {
            // Repetitions in an empty string should be 0.
            Assert.AreEqual(0, "".CountRepetitions());

            // Repetitions in string with no repetitions should be 0.
            Assert.AreEqual(0, "12345abcde".CountRepetitions());

            // Repetitions in string with one island of one repetition should be 1.
            Assert.AreEqual(1, "abccdef".CountRepetitions());

            // Repetitions in string with one island of one repetitions, then two should be 2.
            Assert.AreEqual(2, "abccdefffg".CountRepetitions());

            // Repetitions in string with one island of two repetitions, then one should be 2.
            Assert.AreEqual(2, "abcccdeffg".CountRepetitions());
        }

        [TestMethod]
        public void TestCountConsecutives()
        {
            // Consecutives in an empty string should be 0.
            Assert.AreEqual(0, "".CountConsecutives());

            // Consecutives in string with no consecutives should be 0.
            Assert.AreEqual(0, "24680".CountConsecutives());

            // Consecutives in string with one island of one consecutives should be 1.
            Assert.AreEqual(1, "24679".CountConsecutives());

            // Consecutives in string with one island of one consecutives, then two should be 2.
            Assert.AreEqual(2, "ace12gik123".CountConsecutives());

            // Consecutives in string with one island of two consecutives, then one should be 2.
            Assert.AreEqual(2, "ace123gik12".CountConsecutives());
        }
    }
}
