using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Structs;
using Ferrum.FakeBank.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ferrum.FakeBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoriseController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new { Get = "Authorise Controller", Success = true });
        }
        
        [HttpPost]
        public async Task<AuthoriseResponse> Post([FromBody] AuthoriseRequest request)
        {
            var random = new Random().Next(0, 4);
            if (random == 4)
                throw new Exception("Random Exception");

            await Task.Delay(random * 1000);
            
            var cardNumber = new CardNumber(request.CardNumber);
                        
            if (request.SecurityCode == 200 && cardNumber.IsValid)
                return request.Respond(AuthStatus.Authorised);

            if (request.SecurityCode == 404)
                return request.Respond(AuthStatus.Unknown);

            if (request.SecurityCode == 500)
                throw new Exception();

            if (request.SecurityCode == 501)
                return request.Respond(AuthStatus.Error);

            return request.Respond(AuthStatus.Declined);
        }        
    }
}
