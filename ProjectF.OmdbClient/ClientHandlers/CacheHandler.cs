using System.Net;
using System.Web;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using ProjectF.Cache.Configurations;
using ProjectF.Core.Extensions;
using ProjectF.OmdbClient.Constants;
using ProjectF.OmdbClient.Models.Requests;

namespace ProjectF.OmdbClient.ClientHandlers;

public class CacheHandler(
    IOptions<CacheConfiguration> cacheConfig,
    HybridCache hybridCache) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (DefineCaching(request) is not (not null, not null) cacheDetails)
        {
            //TODO: add log about not caching request
            return await base.SendAsync(request, cancellationToken);
        }

        var content = await hybridCache.GetOrCreateAsync(
            cacheDetails.CacheKey,
            async ct =>
            {
                var response = await base.SendAsync(request, ct);

                return await response.Content.ReadAsStringAsync(ct);
            },
            options: cacheDetails.Options,
            cancellationToken: cancellationToken);

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };
    }

    private (string? CacheKey, HybridCacheEntryOptions? Options) DefineCaching(HttpRequestMessage request)
    {
        string? cacheKey;
        HybridCacheEntryOptions? options;
        
        if (!request.Options.TryGetValue(new HttpRequestOptionsKey<string>(RequestOptions.RequestName),
                out var requestName))
        {
            return (null, null);
        }

        switch (requestName)
        {
            case nameof(Services.OmdbClient.SearchAsync):
                var query = HttpUtility.ParseQueryString(request.RequestUri!.Query);
                var termSuffix = query.Get(CustomAttributeUtility.GetJsonName<SearchQueryModel>(model => model.Term));
                var yearSuffix = query.Get(CustomAttributeUtility.GetJsonName<SearchQueryModel>(model => model.Year));
                var pageSuffix = query.Get(CustomAttributeUtility.GetJsonName<SearchQueryModel>(model => model.Page));
                
                cacheKey = string.Format(CacheKeys.Search, termSuffix, yearSuffix, pageSuffix);
                options = new HybridCacheEntryOptions
                {
                    LocalCacheExpiration =
                        TimeSpan.FromMinutes(cacheConfig.Value.SearchOptions.LocalCacheExpirationInMinutes),
                    Expiration =
                        TimeSpan.FromMinutes(cacheConfig.Value.SearchOptions.DistributedCacheExpirationInMinutes)
                };
                break;
            default:
                return (null, null);
        }
        
        return (cacheKey, options);
    }
}