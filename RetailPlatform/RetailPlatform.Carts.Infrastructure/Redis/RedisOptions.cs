namespace RetailPlatform.Carts.Infrastructure.Redis;

internal class RedisOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";
    public int CartTtlMinutes { get; set; } = 1440; // 24 hours default
}
