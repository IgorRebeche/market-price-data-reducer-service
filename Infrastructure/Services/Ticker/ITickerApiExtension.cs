using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Refit;

namespace Infrastructure.Services.Ticker
{
    public static class ITickerApiExtension
    {
        public static IServiceCollection AddTickerApi(this IServiceCollection services, IConfiguration configuration)
        {
            var resilienceOptions = new ResilienceOptions();
            configuration.GetSection(nameof(ResilienceOptions)).Bind(resilienceOptions);

            var tickerApiUri = configuration.GetSection("TickerApi:Uri").Value;

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>() // Thrown by Polly's TimeoutPolicy if the inner call gets timeout.
                .WaitAndRetryAsync(resilienceOptions.Retry, _ => TimeSpan.FromMilliseconds(resilienceOptions.Wait));

            AsyncTimeoutPolicy<HttpResponseMessage> timeoutPolicy = Policy
                .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(resilienceOptions.Timeout));

            services.AddRefitClient<ITickerApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(tickerApiUri))
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy); // The order of adding is imporant!

            return services;
        }
    }
}
