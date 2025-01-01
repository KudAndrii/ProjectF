using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProjectF.Core.Models;
using ProjectF.Handlers.Queries;
using ProjectF.Handlers.QueryHandlers;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.Handlers;

public static class HandlersRegistrations
{
    public static IServiceCollection WithHandlersServices(this IServiceCollection services)
    {
        services
            .AddScoped<IRequestHandler<SearchMoviesQuery, PagedResult<SearchItemResponseModel>>, SearchMoviesHandler>();

        return services;
    }
}