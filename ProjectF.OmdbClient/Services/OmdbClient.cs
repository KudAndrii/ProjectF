using System.Net.Http.Json;
using ProjectF.OmdbClient.Constants;
using ProjectF.OmdbClient.Extensions;
using ProjectF.OmdbClient.Models.Requests;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.OmdbClient.Services;

public class OmdbClient(HttpClient httpClient)
{
    public async Task<SearchResponseModel> SearchAsync(SearchQueryModel query, CancellationToken cancellationToken)
    {
        var url = new Uri(httpClient.BaseAddress!.ToString(), UriKind.Absolute).AppendQuery(query);
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Options.Set(new HttpRequestOptionsKey<string>(RequestOptions.RequestName), nameof(SearchAsync));
        var response = await httpClient.SendAsync(request, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<SearchResponseModel>(cancellationToken);
        
        return result!;
    }
}