using Asp.Versioning;

namespace ProjectF.Api.Core.Models;

public class ApiContext
{
    public required ApiVersion ApiVersion { get; init; }
}