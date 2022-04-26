using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options
{
    public static class TimeframeExtension
    {
        public static IConfiguration AddTimeframeOptions(this IConfiguration configuration, IServiceCollection service)
        {
            service.Configure<List<TimeframeOptions>>(configuration.GetSection("TimeframeOptions"));
            return configuration;
        }
    }
}