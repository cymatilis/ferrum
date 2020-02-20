using Ferrum.Core.Extensions;
using System;
using System.Linq;

namespace Ferrum.Core.Structs
{
    public static class CardNumberLogic
    {
        internal static byte[] CreateCardNumberByteArray(string cardNumber)
        {
            var noSpaces = cardNumber.Replace(" ", "").Replace("-", "");
            if (noSpaces.Length < 15) throw new ArgumentException("Not long enough.", nameof(cardNumber));
            if (noSpaces.Length > 19) throw new ArgumentException("Too long.", nameof(cardNumber));

            if (!noSpaces.IsNumeric())
                throw new ArgumentException("Contains invalid characters. " +
                    "CardNumber can only contain digits 0-9 and space or dash seperators.", nameof(cardNumber));

            var result = noSpaces.Select(c => byte.Parse(c.ToString())).ToArray();
            return result;
        }

        internal static int GetLuhnCheckValue(this CardNumber cardNumber)
        {
            //taken from https://www.rosettacode.org/wiki/Luhn_test_of_credit_card_numbers#C.23
            
            var result = cardNumber.Digits.Select((d, i) => i % 2 == cardNumber.Digits.Length % 2 ? ((2 * d) % 10) + d / 5 : d).Sum() % 10;

            return result;
        }

        internal static short ParseSecurityCode(int securityCode)
        {
            if (securityCode < 0 || securityCode > 9999)
                throw new ArgumentOutOfRangeException("Security code should be between 000 and 9999");

            return (short)securityCode;
        }

        public static string Last4Digits(this CardNumber cardNumber)
        {
            var result = string.Empty;
            for (var i = cardNumber.Length - 1; i > cardNumber.Length - 4 - 1; i--)
                result = cardNumber.Digits[i] + result;

            return result;
        }
    }
}
