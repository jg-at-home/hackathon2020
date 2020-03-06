using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestUsernameMatching1()
        {
            var regex = @"\B@\w+\b";
            var input = "Hello @lordGoldemort!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual("@lordGoldemort", matches[0].Value);
        }

        [TestMethod]
        public void TestUsernameMatching2()
        {
            var regex = @"\B@[A-Za-z]\w+\b";
            var input = "Hello @lordGoldemort from @alix!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("@lordGoldemort", matches[0].Value);
            Assert.AreEqual("@alix", matches[1].Value);
        }

        [TestMethod]
        public void TestUsernameMatching3()
        {
            var regex = @"\B@[A-Za-z]\w+\b";
            var input = "@lordGoldemort says hi!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual("@lordGoldemort", matches[0].Value);
        }

        [TestMethod]
        public void TestUsernameMatching4()
        {
            var regex = @"\B@[A-Za-z]\w+\b";
            var input = "Hello @ lordGoldemort!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(0, matches.Count);
        }

        [TestMethod]
        public void TestUsernameMatching5()
        {
            var regex = @"\B@[A-Za-z]\w+\b";
            var input = "Hello @2!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(0, matches.Count);
        }
    }
}
