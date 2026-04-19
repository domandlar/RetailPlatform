using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RetailPlatform.Carts.Domain.Entities;
using RetailPlatform.Carts.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace RetailPlatform.Carts.Infrastructure.Redis;

internal class RedisCartRepository : ICartRepository
{
    private readonly IDatabase _db;
    private readonly TimeSpan _ttl;
    private readonly ILogger<RedisCartRepository> _logger;

    //public RedisCartRepository(
    //    IConnectionMultiplexer redis,
    //    IConfiguration configuration,
    //    ILogger<RedisCartRepository> logger)
    //{
    //    var ttlInMinutes = int.Parse(configuration.GetSection("Redis")["CartTtlMinutes"]);
    //    _db = redis.GetDatabase();
    //    _ttl = TimeSpan.FromMinutes(ttlInMinutes);
    //    _logger = logger;
    //}

    public Task DeleteAsync(string userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Cart?> GetAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var value = await _db.StringGetAsync($"cart:{userId}");
            if (value.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<Cart>(value!.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cart for user {UserId}", userId);
            throw;
        }
    }

    public async Task SaveAsync(Cart cart, CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(cart);
            await _db.StringSetAsync($"cart:{cart.UserId}", json, _ttl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save cart for user {UserId}", cart.UserId);
            throw;
        }
    }
}
