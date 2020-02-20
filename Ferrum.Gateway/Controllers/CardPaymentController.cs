using Ferrum.Core.Domain;
using Ferrum.Core.Models;
using Ferrum.Core.ServiceInterfaces;
using Ferrum.Core.Structs;
using Ferrum.Gateway.Authentication;
using Ferrum.Gateway.Data;
using Ferrum.Gateway.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ferrum.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(AuthoriseClient))]
    public class CardPaymentController : ControllerBase
    {
        private readonly GatewayDbContext _dbContext;
        private readonly ICardAuthorisation _cardAuthoriser;

        public CardPaymentController(
            GatewayDbContext dbContext, 
            ICardAuthorisation cardAuthoriser)
        {
            _dbContext = dbContext;
            _cardAuthoriser = cardAuthoriser;
        }
        
        [HttpPost]
        [Route("authorise")]
        public async Task<AuthoriseResponse> AuthoriseTransaction(AuthoriseRequest request)
        {
            var user = RouteData.GetUser();
            var cardNumber = new CardNumber(request.CardNumber);
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            var response = await _cardAuthoriser.AuthoriseAsync(request);
            stopwatch.Stop();
            response.ProcessingTimeMs = Convert.ToInt32(stopwatch.ElapsedMilliseconds);

            var transaction = Transaction.Create(response, cardNumber, user);
            
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            return response;
        }


        [HttpPost]
        [Route("getTransaction")]
        public async Task<IActionResult> GetTransaction(TransactionRequest request)
        {
            var transaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(tx => tx.TransactionId == request.TransactionId);

            if (transaction == null)
                return NotFound(new ErrorResponse { Message = "Transaction not found.", StatusCode = 404 });

            var response = transaction.ToAuthResponse();

            return Ok(response);
        }
    }
}
