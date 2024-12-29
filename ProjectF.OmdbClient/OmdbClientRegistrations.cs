using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProjectF.OmdbClient.ClientHandlers;
using ProjectF.OmdbClient.Configurations;

namespace ProjectF.OmdbClient;

public static class OmdbClientRegistrations
{
    public static IServiceCollection WithOmdbClientServices(this IServiceCollection services)
    {
        services.AddOptions<OmdbConfiguration>().BindConfiguration(nameof(OmdbConfiguration)).ValidateOnStart();
        services.AddTransient<CacheHandler>();
        services.AddTransient<AuthHandler>();
        services.AddTransient<ExceptionHandler>();
        services.AddHttpClient<Services.OmdbClient>((scope, client) =>
            {
                var omdbConfig = scope.GetService<IOptions<OmdbConfiguration>>()!.Value;

                client.BaseAddress = omdbConfig.BaseAddress;
            })
            .AddHttpMessageHandler<CacheHandler>()
            .AddHttpMessageHandler<AuthHandler>()
            .AddHttpMessageHandler<ExceptionHandler>();

        return services;
    }
}