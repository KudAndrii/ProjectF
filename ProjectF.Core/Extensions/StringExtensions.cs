namespace ProjectF.Core.Extensions;

public static class StringExtensions
{
    public static string IfNullOrEmpty(this string str, string defaultValue) =>
        str is { Length: > 0 } ? str : defaultValue;
}