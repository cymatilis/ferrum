using Ferrum.Core.Models;
using Ferrum.Core.Utils;
using Ferrum.Gateway.Data;
using Ferrum.Gateway.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ferrum.Gateway.Authentication
{
    /// <summary>
    /// Simple user authentication to demo action filters.
    /// For authentication in production, I would recommend using Identity Server.
    /// </summary>
    public class AuthoriseClient : ActionFilterAttribute
    {
        private GatewayDbContext _dbContext;

        public AuthoriseClient(GatewayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var matched = false;
            try
            {
                var userRequest = context.ActionArguments["request"] as IUserRequest;
                var userGuid = new Guid(userRequest.UserId);
                var user = _dbContext.UserAccounts.FirstOrDefault(l => l.UserId == userGuid);
                matched = PasswordUtils.Match(userRequest.UserSecret, user.Salt, user.UserSecret);
                if (matched)
                    context.RouteData.DataTokens.Add("UserAccount", user);
            }
            catch (Exception)
            {
                await Unauthorised(context);
                return;
            }

            if (!matched)
            {
                await Unauthorised(context);
                return;
            }
                
            await next();
        }

        private async Task Unauthorised(ActionExecutingContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 401;
            response.Headers.Add("content-type", "application/json");
            var errorModel = new ErrorResponse
            {
                StatusCode = 401,
                Message = "Request to the gateway is unauthorised."
            };            
            await response.WriteAsync(errorModel.AsJsonString());                
        }
    }
}
