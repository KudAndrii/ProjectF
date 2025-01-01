using MediatR;
using ProjectF.Core.Models;
using ProjectF.Handlers.Queries;
using ProjectF.OmdbClient.Exceptions;
using ProjectF.OmdbClient.Models.Requests;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.Handlers.QueryHandlers;

public class SearchMoviesHandler(OmdbClient.Services.OmdbClient omdbClient)
    : IRequestHandler<SearchMoviesQuery, PagedResult<SearchItemResponseModel>>
{
    public async Task<PagedResult<SearchItemResponseModel>> Handle(SearchMoviesQuery request,
        CancellationToken cancellationToken)
    {
        var firstMoviesPage = await omdbClient.SearchAsync(new SearchQueryModel
        {
            Term = request.Term,
            Year = request.Year,
            Page = 1
        }, cancellationToken);

        if (!int.TryParse(firstMoviesPage.TotalResults, out var totalResults))
        {
            throw new OmdbRequestException("Invalid number of results returned by OmdbClient");
        }

        if (firstMoviesPage.FoundItems is not { Count: > 0 })
        {
            return new PagedResult<SearchItemResponseModel>([], request.PageNumber, request.PageSize);
        }
        
        if (totalResults <= firstMoviesPage.FoundItems.Count)
        {
            return new PagedResult<SearchItemResponseModel>(
                firstMoviesPage.FoundItems, request.PageNumber, request.PageSize);
        }

        var totalPages = Math.Ceiling(totalResults / (decimal)firstMoviesPage.FoundItems.Count);
        List<Task<SearchResponseModel>> moviesPageTasks = [];

        for (var page = 2; page < totalPages + 1; page++)
        {
            moviesPageTasks.Add(omdbClient.SearchAsync(new SearchQueryModel
            {
                Term = request.Term,
                Year = request.Year,
                Page = page
            }, cancellationToken));
        }
        
        var restMoviesPages = (await Task.WhenAll(moviesPageTasks))
            .SelectMany(moviesPage => moviesPage.FoundItems ?? []);

        var allMovies = firstMoviesPage.FoundItems.Concat(restMoviesPages).ToList();
        
        return new PagedResult<SearchItemResponseModel>(allMovies, request.PageNumber, request.PageSize);
    }
}