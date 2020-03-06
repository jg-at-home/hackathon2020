using System;
using System.Text.RegularExpressions;
using Helpers;
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

        [TestMethod]
        public void TestUrlMatching1()
        {
            var regex =
                @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?";
            var input = "I went to http://www.foo.com but all I got was a lousy t-shirt";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual("http://www.foo.com", matches[0].Value);
        }

        [TestMethod]
        public void TestUrlMatching2()
        {
            var regex =
                @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?";
            var input = "http://www.foo.com is the place to be!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual("http://www.foo.com", matches[0].Value);
        }

        [TestMethod]
        public void TestUrlMatching3()
        {
            var regex =
                @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?";
            var input = "http://www.foo.com and https://foo.co.uk are the places to be!";
            var matches = Regex.Matches(input, regex);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("http://www.foo.com", matches[0].Value);
            Assert.AreEqual("https://foo.co.uk", matches[1].Value);
        }

        [TestMethod]
        public void TestDomainExtraction1()
        {
            var input = "http://www.foo.com";
            var domain = UrlHelpers.ExtractDomainFromUrl(input);
            Assert.AreEqual("www.foo.com", domain);
        }

        [TestMethod]
        public void TestDomainExtraction2()
        {
            var input = "http://foo.com/work";
            var domain = UrlHelpers.ExtractDomainFromUrl(input);
            Assert.AreEqual("foo.com", domain);
        }

        [TestMethod]
        public void TestDomainExtraction3()
        {
            var input = "https://foo.com:2020";
            var domain = UrlHelpers.ExtractDomainFromUrl(input);
            Assert.AreEqual("foo.com", domain);
        }
    }
}
