using MediatR;
using ProjectF.Core.Models;
using ProjectF.Handlers.Queries;
using ProjectF.OmdbClient.Models.Requests;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.Handlers.QueryHandlers;

public class SearchMoviesHandler(OmdbClient.Services.OmdbClient omdbClient)
    : IRequestHandler<SearchMoviesQuery, PagedResult<SearchItemResponseModel>>
{
    public async Task<PagedResult<SearchItemResponseModel>> Handle(SearchMoviesQuery request,
        CancellationToken cancellationToken)
    {
        var responseModel = await omdbClient.SearchAsync(new SearchQueryModel
        {
            Term = request.Term,
            Year = request.Year,
            Page = request.Page,
        }, cancellationToken);

        var result = new PagedResult<SearchItemResponseModel>(responseModel.FoundItems.Count, request.Page,
            responseModel.FoundItems.Count);
        
        return result;
    }
}