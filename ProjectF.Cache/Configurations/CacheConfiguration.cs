using System.ComponentModel.DataAnnotations;

namespace ProjectF.Cache.Configurations;

public class CacheConfiguration
{
    [Required]
    public required CacheOptions SearchOptions { get; init; }
}

public class CacheOptions
{
    [Range(1, int.MaxValue)]
    public int LocalCacheExpirationInMinutes { get; init; }
    
    [Range(1, int.MaxValue)]
    public int DistributedCacheExpirationInMinutes { get; init; }
}