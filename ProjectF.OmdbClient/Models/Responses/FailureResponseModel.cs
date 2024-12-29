using System.Text.Json.Serialization;

namespace ProjectF.OmdbClient.Models.Responses;

public class FailureResponseModel
{
    [JsonPropertyName("Response")]
    public string? Response { get; init; }
    
    [JsonPropertyName("Error")]
    public string? Message { get; init; }
}