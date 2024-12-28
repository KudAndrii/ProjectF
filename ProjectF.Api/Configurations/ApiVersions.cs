using Asp.Versioning;

namespace ProjectF.Api.Configurations;

public static class ApiVersions
{
    public static readonly ApiVersion V09 = new(0.9, "deprecated");
    public static readonly ApiVersion V1 = new(1.0, "actual");
    public static readonly ApiVersion V2 = new(2.0, "coming");
}