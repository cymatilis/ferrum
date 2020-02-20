using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Structs;
using System;

namespace Ferrum.Core.Models
{
    public class AuthoriseResponse
    {
        public Guid TransactionId { get; set; }

        public DateTime TimeStampUtc { get; set; }

        public AuthStatus AuthStatus { get; set; }

        public string CardNumberEnding { get; set; }

        public CardNetwork CardNetwork { get; set; }

        public string CurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public int RetryAttempts { get; set; }

        public int ProcessingTimeMs { get; set; }

        public static AuthoriseResponse CreateFailedResponse(AuthoriseRequest request, int retryAttempts)
        {
            var cardNumber = new CardNumber(request.CardNumber);

            var result = new AuthoriseResponse
            {
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode,
                AuthStatus = AuthStatus.Error,
                TransactionId = Guid.NewGuid(),
                CardNetwork = cardNumber.CardNetwork,
                CardNumberEnding = cardNumber.Last4Digits(),
                RetryAttempts = retryAttempts,
                TimeStampUtc = DateTime.UtcNow
            };

            return result;
        }        
    }
}
