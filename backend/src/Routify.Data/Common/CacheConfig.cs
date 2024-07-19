namespace Routify.Data.Common;

public record CacheConfig
{
    public bool Enabled { get; set; }
    public int Expiration { get; set; }
}