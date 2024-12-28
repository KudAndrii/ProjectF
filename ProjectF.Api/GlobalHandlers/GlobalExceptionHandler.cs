using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Core.Extensions;

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
            NotImplementedException notImplementedException => new ProblemDetails
            {
                Type = nameof(StatusCodes.Status500InternalServerError),
                Status = StatusCodes.Status500InternalServerError,
                Title = nameof(NotImplementedException),
                Detail = notImplementedException.Message.IfNullOrEmpty("Not yet implemented")
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