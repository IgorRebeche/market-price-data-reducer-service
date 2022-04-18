using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Options;
using Domain.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Database.MongoDb;

namespace Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            config.AddTimeframeOptions(services);
            config.AddAssetOptions(services);
            services.AddScoped<ITickerRepository, TickerRepository>();
            services.AddScoped<ICandleRepository, CandleRepository>();
            services.Configure<MarketPriceLakeDatabaseConfiguration>(config.GetSection("MarketPriceLakeDatabase"));
            return services;
        }
    }
}
