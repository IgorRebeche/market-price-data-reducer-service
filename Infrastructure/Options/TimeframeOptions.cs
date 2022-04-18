using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options
{
    public static class TimeframeOptions
    {
        public static IConfiguration AddTimeframeOptions(this IConfiguration configuration, IServiceCollection service)
        {
            service.Configure<List<Timeframe>>(configuration.GetSection("TimeframeOptions"));
            return configuration;
        }
    }
}