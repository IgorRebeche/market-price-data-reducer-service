using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Options;
using Domain.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Database.MongoDb;
using Infrastructure.Services.Ticker;
using Application.Services.Ticker;

namespace Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            config.AddTimeframeOptions(services);
            config.AddAssetOptions(services);
            config.AddResilienceOptions(services);

            services.AddTickerApi(config);
            services.AddScoped<ICandleRepository, CandleRepository>();
            services.AddScoped<ITickerService, TickerService>();
            services.Configure<MarketPriceLakeDatabaseConfiguration>(config.GetSection("MarketPriceLakeDatabase"));
            return services;
        }
    }
}
