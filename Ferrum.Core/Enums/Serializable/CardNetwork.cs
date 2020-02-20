using Ferrum.Core.Extensions;
using Ferrum.Core.Structs;

namespace Ferrum.Core.Enums.Serializable
{
    public enum CardNetwork
    {
        Unknown = 0,
        Visa = 1,
        MasterCard = 2,
        Amex = 3,
        Discover = 4
    }

    internal static class CardNetworkExtensions
    {
        internal static CardNetwork GetCardNetwork(this CardNumber cardNumber)
        {
            var firstDigit = cardNumber.Digits[0];
            var secondDigit = cardNumber.Digits[1];
            var length = cardNumber.Digits.Length;

            if (firstDigit == 4 && length == 16)
                return CardNetwork.Visa;

            if (firstDigit == 5 && secondDigit <= 5 && length == 16)
                return CardNetwork.MasterCard;

            if (firstDigit == 3 && (secondDigit == 4 || secondDigit == 7) && length == 15)
                return CardNetwork.Amex;

            if (firstDigit == 6 && secondDigit.In(0, 2, 4, 5))
                return CardNetwork.Discover;

            return CardNetwork.Unknown;
        }
    }
}
