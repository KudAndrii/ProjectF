using System.ComponentModel.DataAnnotations;

namespace ProjectF.OmdbClient.Configurations;

public class OmdbConfiguration
{
    [Required]
    [Url]
    public required Uri BaseAddress { get; init; }

    [Required]
    [MinLength(1)]
    public required string ApiKey { get; init; }
}