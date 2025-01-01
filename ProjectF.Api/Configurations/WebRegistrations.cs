using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectF.Api.GlobalHandlers;

namespace ProjectF.Api.Configurations;

public static class WebRegistrations
{
    public static IServiceCollection WithApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersions.V09;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        });

        return services;
    }

    public static IServiceCollection WithHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("ProjectF")!)
            .AddRedis(configuration.GetConnectionString("DistributedCache")!, failureStatus: HealthStatus.Degraded)
            .AddCheck<OmdbHealthCheck>("Omdb");
        
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
    
    public static IServiceCollection WithMediator(this IServiceCollection services) => services
        .AddMediatR(mr => mr.RegisterServicesFromAssemblyContaining<Program>());

    public static ApiVersionSet UseVersioning(this WebApplication app) => app
        .NewApiVersionSet()
        .HasApiVersion(ApiVersions.V09)
        .ReportApiVersions()
        .Build();
}