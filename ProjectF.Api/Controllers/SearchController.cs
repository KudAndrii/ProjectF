using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Handlers.Queries;

namespace ProjectF.Api.Controllers;

[Route("api/search")]
[ApiVersion("0.9-dev")]
public class SearchController(ISender sender) : BaseController
{
    [HttpGet]
    public async Task<IResult> Search([FromQuery] string term, [FromQuery] int? year,
        [FromQuery] [Range(1, 100)] int page = 1, CancellationToken cancellationToken = default)
    {
        var query = new SearchMoviesQuery(term, year, page);
        var result = await sender.Send(query, cancellationToken);
        
        return TypedResults.Ok(result);
    }
}