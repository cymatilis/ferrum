using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Models;
using Ferrum.Core.Structs;
using System;

namespace Ferrum.Core.Domain
{
    public class Transaction
    {
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public CardNumber CardNumber { set; private get; }
        public string CardNumberEnding { get; set; }
        public AuthStatus AuthStatus { get; set; }
        public CardNetwork CardNetwork { get; set; }
        public DateTime TimeStampUtc { get; set; }

        //these could be pushed out and expanded into a seperate logging/audit table:
        public int RetryAttempts { get; set; }
        public int ProcessingTimeMs { get; set; }

        public static Transaction Create(AuthoriseResponse authoriseResponse, CardNumber cardNumber, UserAccount userAccount)
        {
            var transaction = new Transaction
            {
                TransactionId = authoriseResponse.TransactionId,
                Amount = authoriseResponse.Amount,
                AuthStatus = authoriseResponse.AuthStatus,
                CardNetwork = authoriseResponse.CardNetwork,
                CardNumber = cardNumber,
                CardNumberEnding = cardNumber.Last4Digits(),
                ClientId = userAccount.ClientId,
                UserId = userAccount.Id,
                CurrencyCode = authoriseResponse.CurrencyCode,
                TimeStampUtc = authoriseResponse.TimeStampUtc,
                RetryAttempts = authoriseResponse.RetryAttempts,
                ProcessingTimeMs = authoriseResponse.ProcessingTimeMs
            };

            return transaction;
        }

        public AuthoriseResponse ToAuthResponse()
        {
            var response = new AuthoriseResponse
            {
                Amount = Amount,
                AuthStatus = AuthStatus,
                CardNetwork = CardNetwork,
                CardNumberEnding = CardNumberEnding,
                CurrencyCode = CurrencyCode,
                TimeStampUtc = TimeStampUtc,
                RetryAttempts = RetryAttempts,
                ProcessingTimeMs = ProcessingTimeMs,
                TransactionId = TransactionId
            };

            return response;
        }
    }
}
