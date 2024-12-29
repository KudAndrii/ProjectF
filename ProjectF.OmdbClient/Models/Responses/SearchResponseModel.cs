using System.Text.Json.Serialization;

namespace ProjectF.OmdbClient.Models.Responses;

public class SearchResponseModel
{
    [JsonPropertyName("Search")]
    public ICollection<SearchItemResponseModel>? FoundItems { get; init; }

    [JsonPropertyName("totalResults")]
    public string? TotalResults { get; init; }
}