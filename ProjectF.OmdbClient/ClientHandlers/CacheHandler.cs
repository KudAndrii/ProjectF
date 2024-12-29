using System.Net;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Caching.Hybrid;
using ProjectF.Core.Extensions;
using ProjectF.OmdbClient.Models.Requests;

namespace ProjectF.OmdbClient.ClientHandlers;

public class CacheHandler(HybridCache hybridCache) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var query = HttpUtility.ParseQueryString(request.RequestUri!.Query);
        var termKey = CustomAttributeUtility.GetAttributeValue<JsonPropertyNameAttribute, SearchQueryModel>(
            attr => attr.Name, model => model.Term);
        var termValue = query.Get(termKey);
        
        var content = await hybridCache.GetOrCreateAsync($"search_{termValue}",
            async ct =>
            {
                var response = await base.SendAsync(request, ct);

                return await response.Content.ReadAsStringAsync(ct);
            },
            tags: ["search"],
            cancellationToken: cancellationToken);
        
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };
    }
}