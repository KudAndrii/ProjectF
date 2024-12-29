using System.Text.Json.Serialization;

namespace ProjectF.OmdbClient.Models.Responses;

public class SearchItemResponseModel
{
    [JsonPropertyName("Title")]
    public string? Title { get; init; }

    [JsonPropertyName("Year")]
    public string? Year { get; init; }

    [JsonPropertyName("imdbID")]
    public string? ImdbId { get; init; }

    [JsonPropertyName("Type")]
    public string? Type { get; init; }

    [JsonPropertyName("Poster")]
    public string? Poster { get; init; }
}