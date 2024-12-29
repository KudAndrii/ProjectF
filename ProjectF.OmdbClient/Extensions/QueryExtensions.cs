using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Web;

namespace ProjectF.OmdbClient.Extensions;

public static class QueryExtensions
{
    /// <summary>
    /// Appends a single query parameter to the URI.
    /// </summary>
    public static Uri AppendQuery(this Uri uri, string parameterValue,
        [CallerArgumentExpression("parameterValue")]
        string? parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(parameterValue) || string.IsNullOrWhiteSpace(parameterName))
        {
            return uri;
        }

        var uriBuilder = new UriBuilder(uri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query[parameterName] = parameterValue;

        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }

    /// <summary>
    /// Appends an object's properties as query parameters to the URI.
    /// </summary>
    public static Uri AppendQuery<TQuery>(this Uri uri, TQuery @params)
    {
        if (@params is null)
        {
            return uri;
        }
        
        var uriBuilder = new UriBuilder(uri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        var properties = @params.GetType()
            .GetProperties()
            .Where(prop => 
                prop.PropertyType.IsPrimitive || 
                prop.PropertyType == typeof(string) ||
                (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && 
                 Nullable.GetUnderlyingType(prop.PropertyType)!.IsPrimitive));
        
        foreach (var property in properties)
        {
            var paramName = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).FirstOrDefault()
                is JsonPropertyNameAttribute jsonProperty
                ? jsonProperty.Name
                : property.Name;

            var value = property.GetValue(@params)?.ToString();

            if (!string.IsNullOrWhiteSpace(value))
            {
                query[paramName] = value;
            }
        }

        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }
}