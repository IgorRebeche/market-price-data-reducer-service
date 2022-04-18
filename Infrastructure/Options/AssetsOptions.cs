using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options
{
    public static class AssetsOptions
    {
        public static IConfiguration AddAssetOptions(this IConfiguration configuration, IServiceCollection service)
        {
            service.Configure<List<Asset>>(configuration.GetSection("Assets"));
            return configuration;
        }
    }
}