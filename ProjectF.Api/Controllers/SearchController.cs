using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProjectF.OmdbClient.Models.Requests;

namespace ProjectF.Api.Controllers;

[Route("api/search")]
[ApiVersion("1.0-actual")]
public class SearchController(OmdbClient.Services.OmdbClient omdbClient) : BaseController
{
    [HttpGet]
    public async Task<IResult> Search(
        [FromQuery] string term, [FromQuery] int? year, [FromQuery] int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await omdbClient.Search(new SearchQueryModel
        {
            Term = term,
            Year = year,
            Page = page
        }, cancellationToken);

        return TypedResults.Ok(result);
    }
}