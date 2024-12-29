using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ProjectF.OmdbClient.Exceptions;
using ProjectF.OmdbClient.Models.Responses;

namespace ProjectF.OmdbClient.ClientHandlers;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? bodyString = null;

        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            bodyString = await response.Content.ReadAsStringAsync(cancellationToken);
            var bodyObj = JsonSerializer.Deserialize<FailureResponseModel>(bodyString);

            if (bodyObj is not { Response: "True" })
            {
                throw new OmdbRequestException(bodyObj?.Message, null, HttpStatusCode.BadRequest);
            }

            return response;
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "OMDB request ended with error: {Message}, status code: {Status}",
                exception.Message, exception.StatusCode);

            throw OmdbRequestException.From(exception);
        }
        catch (Exception exception) when (exception.InnerException is TimeoutException)
        {
            logger.LogError(exception, "OMDB request ended with error: {Message}, status code: {Status}",
                exception.Message, HttpStatusCode.RequestTimeout);

            throw new OmdbRequestException(exception.Message, exception.InnerException,
                HttpStatusCode.RequestTimeout);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "OMDB request ended with error: {Message}", exception.Message);

            throw;
        }
        finally
        {
            using var loggerScope = logger.BeginScope(
                new { Path = request.RequestUri?.ToString() ?? string.Empty, Response = bodyString ?? string.Empty });

            logger.LogInformation("{Method} OMDB request has been executed", request.Method);
        }
    }
}