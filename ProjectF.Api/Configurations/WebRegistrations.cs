using Asp.Versioning;
using Asp.Versioning.Builder;
using ProjectF.Api.GlobalHandlers;

namespace ProjectF.Api.Configurations;

public static class WebRegistrations
{
    public static IServiceCollection WithApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersions.V1;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        });

        return services;
    }

    public static IServiceCollection WithHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("ProjectF")!)
            .AddRedis(configuration.GetConnectionString("DistributedCache")!);
        
        return services;
    }

    public static IServiceCollection WithExceptionHandlers(this IServiceCollection services) => services
        .AddExceptionHandler<GlobalExceptionHandler>()
        .AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });

    public static ApiVersionSet UseVersioning(this WebApplication app) => app
        .NewApiVersionSet()
        .HasApiVersion(ApiVersions.V09)
        .HasApiVersion(ApiVersions.V1)
        .HasApiVersion(ApiVersions.V2)
        .HasDeprecatedApiVersion(ApiVersions.V09)
        .ReportApiVersions()
        .Build();
}