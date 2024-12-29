using System.Text.Json.Serialization;

namespace ProjectF.OmdbClient.Models.Requests;

public class SearchQueryModel
{
    /// <summary>
    /// Movie title to search for.
    /// </summary>
    [JsonPropertyName("s")]
    public string? Term { get; set; }

    /// <summary>
    /// Type of result to return.
    /// </summary>
    /// <example><c>movie</c> <c>series</c> <c>episode</c>
    /// </example>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Year of release.
    /// </summary>
    [JsonPropertyName("y")]
    public int? Year { get; set; }

    /// <summary>
    /// Page number to return.
    /// </summary>
    /// <example>
    /// Range <c>1-100</c>
    /// </example>
    [JsonPropertyName("page")]
    public int? Page { get; set; } = 1;
}