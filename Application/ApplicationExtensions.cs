using Application.UseCases.CreateCandlestick;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ICreateCandleStickUseCase, CreateCandleStickUseCase>();
            return services;
        }
    }
}
