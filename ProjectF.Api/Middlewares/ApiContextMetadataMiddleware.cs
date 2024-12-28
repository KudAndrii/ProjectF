using ProjectF.Api.Core.Models;

namespace ProjectF.Api.Middlewares;

public class ApiContextMetadataMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Features.Set(new ApiContext
        {
            ApiVersion = context.GetRequestedApiVersion()
        });

        await next(context);
    }
}