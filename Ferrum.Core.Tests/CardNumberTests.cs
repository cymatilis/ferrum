using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Structs;
using NUnit.Framework;
using System;

namespace Ferrum.Core.Tests
{
    public class CardNumberTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InvalidInstantiaionThrowsException()
        {
            static void invalidChars() => new CardNumber("1234x1234x1234x1234");
            static void tooLong() => new CardNumber("1234-1234-1234-1234-1234");
            static void tooShort() => new CardNumber("1234 1234 1234");

            Assert.Throws<ArgumentException>(invalidChars);
            Assert.Throws<ArgumentException>(tooLong);
            Assert.Throws<ArgumentException>(tooShort);
        }

        [Test]
        public void CardNumberLength()
        {
            var _16_digit = new CardNumber("1234 1234 1234 1234");
            var _15_digit = new CardNumber("123412341234123");
            var _19_digit = new CardNumber("1234 1234 1234 1234 123");

            Assert.AreEqual(16, _16_digit.Length);
            Assert.AreEqual(15, _15_digit.Length);
            Assert.AreEqual(19, _19_digit.Length);
        }

        [Test]
        public void AmexValidityTests()
        {
            var validAmex1 = new CardNumber("3434 783469 23497");
            var validAmex2 = new CardNumber("3773 500091 12107");
            var invalidAmex = new CardNumber("3434 783469 23498");
            var wrongStartsWith = new CardNumber("3534 783469 23498");
            var wrongLength = new CardNumber("3534 7834690 23498");

            Assert.AreEqual(CardNetwork.Amex, validAmex1.CardNetwork);
            Assert.AreEqual(CardNetwork.Amex, validAmex2.CardNetwork);
            Assert.AreEqual(CardNetwork.Amex, invalidAmex.CardNetwork);
            Assert.AreEqual(CardNetwork.Unknown, wrongStartsWith.CardNetwork);
            Assert.AreEqual(CardNetwork.Unknown, wrongLength.CardNetwork);

            Assert.IsTrue(validAmex1.IsValid);
            Assert.IsTrue(validAmex2.IsValid);
            Assert.IsFalse(invalidAmex.IsValid);
        }
        
        [Test]
        public void MasterCardValidityTests()
        {
            var validMc = new CardNumber("5451564343386429");
            var invalidMc = new CardNumber("5451564343386420");
            var wrongStartsWith = new CardNumber("7451564343386429");
            var wrongLength = new CardNumber("545156434338642");

            Assert.AreEqual(CardNetwork.MasterCard, validMc.CardNetwork);
            Assert.AreEqual(CardNetwork.MasterCard, invalidMc.CardNetwork);
            Assert.AreEqual(CardNetwork.Unknown, wrongStartsWith.CardNetwork);
            Assert.AreEqual(CardNetwork.Unknown, wrongLength.CardNetwork);

            Assert.IsTrue(validMc.IsValid);
            Assert.IsFalse(invalidMc.IsValid);
            Assert.IsFalse(wrongStartsWith.IsValid);
            Assert.IsFalse(wrongLength.IsValid);
        }

        [Test]
        public void VisaValidityTests()
        {
            var validVisa = new CardNumber("4234 5678 9012 3456");
            var invalidVisa = new CardNumber("4234 5678 9012 3457");
            var wrongStartsWith = new CardNumber("3234 5678 9012 3456");
            var wrongLength = new CardNumber("4234 567 9012 3456");

            Assert.AreEqual(CardNetwork.Visa, validVisa.CardNetwork);
            Assert.AreEqual(CardNetwork.Visa, invalidVisa.CardNetwork);
            Assert.AreEqual(CardNetwork.Unknown, wrongStartsWith.CardNetwork);
            Assert.AreEqual(CardNetwork.Unknown, wrongLength.CardNetwork);

            Assert.IsTrue(validVisa.IsValid);
            Assert.IsFalse(invalidVisa.IsValid);
            Assert.IsFalse(wrongStartsWith.IsValid);
            Assert.IsFalse(wrongLength.IsValid);
        }
    }
}