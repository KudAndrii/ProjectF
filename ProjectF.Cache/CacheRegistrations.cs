using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectF.Cache.Configurations;

namespace ProjectF.Cache;

public static class CacheRegistrations
{
    public static IServiceCollection WithCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CacheConfiguration>().BindConfiguration(nameof(CacheConfiguration)).ValidateOnStart();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("DistributedCache");
        });
        
#pragma warning disable EXTEXP0018
        services.AddHybridCache();
#pragma warning restore EXTEXP0018
        
        return services;
    }
}