using Ferrum.Core.Utils;
using NUnit.Framework;
using System;
using System.Linq;
namespace Ferrum.Core.Tests
{
    [TestFixture]
    public class PasswordUtilTests
    {
        private static string _plainPassword = "plainPassword1$";
        private static string _salt = "salt001";
        private string _hashedPassword;

        [SetUp]
        public void Setup()
        { 
            _hashedPassword = PasswordUtils.HashPassword(_plainPassword, _salt);
        }

        [Test]
        public void TestMatchPasses()
        {
            var match = PasswordUtils.Match(_plainPassword, _salt, _hashedPassword);
            Assert.IsTrue(match);
        }

        [Test]
        public void TestMatchFails()
        {
            var slightVariation = _plainPassword.Replace('$','S');
            var match = PasswordUtils.Match(slightVariation, _salt, _hashedPassword);
            Assert.IsFalse(match);
        }

        [Test]
        public void MatchWithNullThrowsArgumentException()
        {
            static void action() => PasswordUtils.Match("asdfsafd", null, "sadfadsfa");

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void Generates100PercUniquePasswords()
        {
            const int numberToGenerate = 1000;

            var passwordArray = Enumerable.Range(1, numberToGenerate)
                .Select(i => PasswordUtils.GeneratePassword())
                .ToArray();

            var count = passwordArray.Distinct().Count();
           
            Assert.AreEqual(count / numberToGenerate, 1);     
        }

        [Test]
        public void GeneratesAtLeast99PercUniqueSalts()
        {
            const int numberToGenerate = 1000;
            
            var saltArray = Enumerable.Range(1, numberToGenerate)
                .Select(i => PasswordUtils.GenerateSalt())
                .ToArray();

            var count = saltArray.Distinct().Count();
           
            var perc = ((double)count / numberToGenerate) * 100;

            Assert.GreaterOrEqual(perc, 99.9);     
        }
    }
}
