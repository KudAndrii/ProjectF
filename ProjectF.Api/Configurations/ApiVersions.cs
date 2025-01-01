using Asp.Versioning;

namespace ProjectF.Api.Configurations;

public static class ApiVersions
{
    public static readonly ApiVersion V09 = new(0.9, "dev");
}