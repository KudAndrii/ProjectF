namespace ProjectF.Core.Constants;

public static class EnvironmentAccessor
{
    public static string Name => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
}