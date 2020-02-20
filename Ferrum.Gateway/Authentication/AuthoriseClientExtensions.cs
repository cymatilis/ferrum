using Ferrum.Core.Domain;
using Microsoft.AspNetCore.Routing;
using System;

namespace Ferrum.Gateway.Authentication
{
    public static class AuthoriseClientExtensions
    {
        public static UserAccount GetUser(this RouteData routeData)
        {
            var success = routeData.DataTokens.TryGetValue("UserAccount", out object userObj);
            if (!success || !(userObj is UserAccount user))
                throw new Exception($"No user found in the route data dictionary. Have you used the ServiceFilter attribute for {nameof(AuthoriseClient)}?");

            return user;
        }
    }
}
