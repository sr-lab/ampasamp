using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampasamp.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
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
