using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;

namespace Ferrum.Gateway.ErrorHandling
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app /* Need an ILogger */)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //user logger here instead to log all exceptions;
                        Debug.Fail(contextFeature.Error.Message);

                        await context.Response.WriteAsync(new ErrorResponse
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.AsJsonString());
                    }
                });
            });
        }
    }
}
