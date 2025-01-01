using MediatR;
using ProjectF.Core.Models;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.Handlers.Queries;

public record SearchMoviesQuery(string Term, int? Year, int PageNumber, int PageSize)
    : IRequest<PagedResult<SearchItemResponseModel>>;