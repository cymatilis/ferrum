using Polly;

namespace Ferrum.Gateway.Integrations.Polly
{
    public static class PollyContextExtensions
    {
        private static string Index = "RetryCounter";

        public static Context IncrementRetryCount(this Context pollyContext)
        {
            pollyContext[Index] = pollyContext.ContainsKey(Index) ? (int)pollyContext[Index] + 1 : 1;
            return pollyContext;
        }

        public static int GetRetryCount(this Context pollyContext)
        {
            var counter = pollyContext.ContainsKey(Index) ? pollyContext[Index] as int? : 0;
            return counter ?? 0;           
        }
    }
}
