using System;
using System.Collections.Generic;
using System.Text;
using Ferrum.Core.Domain;
using Ferrum.Core.Models;
using Ferrum.Core.Structs;

namespace Ferrum.Gateway.Integrations.FakeBank
{
    /*public static class ResponseMapping
    {
        public static Transaction MakeTransaction(AuthoriseResponse authoriseResponse, CardNumber cardNumber, UserAccount userAccount, long processingTimeMs)
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
                ProcessingTimeMs = Convert.ToInt32(processingTimeMs)
            };

            return transaction;
        }
    }*/
}
