using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Models;
using Ferrum.Core.ServiceInterfaces;
using Ferrum.Core.Utils;
using Ferrum.Gateway.Integrations.Polly;
using Microsoft.Extensions.Configuration;
using Polly;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ferrum.Gateway.Integrations.FakeBank
{
    public class FakeBankAuthorisation : ICardAuthorisation
    {
        private readonly HttpClient _client;
                
        public FakeBankAuthorisation(HttpClient httpClient, IConfiguration configuration)
        {
            _client = httpClient;
                       
            var endpoint = configuration.GetSection("ConsumeServiceEndpoints")[nameof(FakeBankAuthorisation)];
            _client.BaseAddress = new Uri(endpoint);
        }

        public async Task<AuthoriseResponse> AuthoriseAsync(AuthoriseRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var pollyContext = new Context();
            
            var httpResponse = await PolicyHandlers.CountRetryPolicy()
                .ExecuteAsync(c => _client.PostAsync("api/authorise", content), pollyContext);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorResponse = AuthoriseResponse.CreateFailedResponse(
                    request, pollyContext.GetRetryCount());
                
                return errorResponse;
            }

            var result = await httpResponse.Content.DeserializeAsync<AuthoriseResponse>();
            result.RetryAttempts = pollyContext.GetRetryCount();

            return result;
        }
    }
}
