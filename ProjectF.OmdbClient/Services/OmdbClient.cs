using System.Net.Http.Json;
using ProjectF.OmdbClient.Extensions;
using ProjectF.OmdbClient.Models.Requests;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.OmdbClient.Services;

public class OmdbClient(HttpClient httpClient)
{
    public async Task<SearchResponseModel> Search(SearchQueryModel query, CancellationToken cancellationToken)
    {
        var url = new Uri(httpClient.BaseAddress!.ToString(), UriKind.Absolute).AppendQuery(query);
        var response = await httpClient.GetAsync(url, cancellationToken);

        return (await response.Content.ReadFromJsonAsync<SearchResponseModel>(cancellationToken))!;
    }
}