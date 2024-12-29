using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Core.Extensions;
using ProjectF.OmdbClient.Exceptions;

namespace ProjectF.Api.GlobalHandlers;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IConfiguration config,
    IWebHostEnvironment webHostEnvironment,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            NotImplementedException => new ProblemDetails
            {
                Type = nameof(StatusCodes.Status500InternalServerError),
                Status = StatusCodes.Status500InternalServerError,
                Title = nameof(NotImplementedException),
                Detail = exception.Message.IfNullOrEmpty("Not yet implemented")
            },
            
            OmdbRequestException { StatusCode: HttpStatusCode.RequestTimeout } => new ProblemDetails
            {
                Type = nameof(StatusCodes.Status504GatewayTimeout),
                Status = StatusCodes.Status504GatewayTimeout,
                Title = nameof(OmdbRequestException),
                Detail = exception.Message
            },
            
            OmdbRequestException => new ProblemDetails
            {
                Type = nameof(StatusCodes.Status502BadGateway),
                Status = StatusCodes.Status502BadGateway,
                Title = nameof(OmdbRequestException),
                Detail = exception.Message
            },

            _ => new ProblemDetails
            {
                Type = nameof(StatusCodes.Status500InternalServerError),
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unhandled server error",
                Detail = exception.Message
            }
        };

        if (webHostEnvironment.IsDevelopment() &&
            config.GetValue<bool>("UseProblemPointer") &&
            new StackTrace(exception, true).GetFrame(0) is { } firstFrame)
        {
            problemDetails.Extensions.TryAdd("problemPointer", firstFrame.ToString());
        }

        if (problemDetails.Status.HasValue)
        {
            httpContext.Response.StatusCode = problemDetails.Status.Value;
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}