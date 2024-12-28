using Microsoft.AspNetCore.Mvc;
using ProjectF.Api.Core.Models;

namespace ProjectF.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    private ApiContext? _apiContext;
    protected ApiContext ApiContext => _apiContext ??= HttpContext.Features.Get<ApiContext>()!;
}