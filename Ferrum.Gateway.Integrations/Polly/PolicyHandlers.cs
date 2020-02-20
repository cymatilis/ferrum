using Polly;
using Polly.Extensions.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Ferrum.Gateway.Integrations.Polly
{
    public static class PolicyHandlers
    {
        private static readonly int[] RetryIntervals = { 50, 100, 250, 500, 1000, 1500 };

        public static IAsyncPolicy<HttpResponseMessage> CountRetryPolicy()
        {
            var result = HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(retryCount: 10, sleepDurationProvider: PredefinedCapped, onRetry: CountRetries);

            return result;
        }

        private static TimeSpan PredefinedCapped(int retryAttempt)
        {
            var ms = retryAttempt > RetryIntervals.Length ?
                RetryIntervals.Last() : RetryIntervals[retryAttempt - 1];

            return TimeSpan.FromMilliseconds(ms);
        }

        private static void CountRetries(DelegateResult<HttpResponseMessage> message, TimeSpan timeSpan, Context pollyContext)
        {
            pollyContext.IncrementRetryCount();
        }
    }
}
