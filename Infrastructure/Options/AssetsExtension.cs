using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options
{
    public static class AssetsExtension
    {
        public static IConfiguration AddAssetOptions(this IConfiguration configuration, IServiceCollection service)
        {
            service.Configure<List<AssetOptions>>(configuration.GetSection("Assets"));
            return configuration;
        }
    }
}