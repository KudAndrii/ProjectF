using Microsoft.Extensions.Options;
using ProjectF.OmdbClient.Configurations;
using ProjectF.OmdbClient.Extensions;

namespace ProjectF.OmdbClient.ClientHandlers;

public class AuthHandler(IOptions<OmdbConfiguration> omdbConfig) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.RequestUri = request.RequestUri!.AppendQuery(omdbConfig.Value.ApiKey, "apiKey");
        
        return base.SendAsync(request, cancellationToken);
    }
}