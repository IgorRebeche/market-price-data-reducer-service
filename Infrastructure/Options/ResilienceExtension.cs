using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options
{
    public static class ResilienceExtension
    {
        public static IConfiguration AddResilienceOptions(this IConfiguration configuration, IServiceCollection service)
        {
            service.Configure<ResilienceOptions>(configuration.GetSection(nameof(ResilienceOptions)));
            return configuration;
        }
    }
}
