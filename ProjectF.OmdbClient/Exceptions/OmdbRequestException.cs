using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;

namespace ProjectF.OmdbClient.Exceptions;

public class OmdbRequestException(string? message, Exception? inner, HttpStatusCode? statusCode)
    : HttpRequestException(message, inner, statusCode)
{
    public static OmdbRequestException From(HttpRequestException exception) =>
        new(exception.Message, exception.InnerException, exception.StatusCode);

    public static void ThrowIfNull([NotNull] object? response,
        [CallerArgumentExpression("response")] string? paramName = null)
    {
        if (response is not null)
        {
            return;
        }

        throw new OmdbRequestException($"Unexpected null response: {paramName}", null, HttpStatusCode.NoContent);
    }
}