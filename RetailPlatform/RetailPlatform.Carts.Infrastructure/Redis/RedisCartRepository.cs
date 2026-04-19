using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RetailPlatform.Carts.Domain.Entities;
using RetailPlatform.Carts.Domain.Repositories;
using StackExchange.Redis;

namespace RetailPlatform.Carts.Infrastructure.Redis;

internal class RedisCartRepository : ICartRepository
{
    private readonly IDatabase _db;
    private readonly TimeSpan _ttl;
    private readonly ILogger<RedisCartRepository> _logger;

    private static string CartKey(string userId) => $"cart:{userId}";

    public RedisCartRepository(
        IConnectionMultiplexer redis,
        IConfiguration configuration,
        ILogger<RedisCartRepository> logger)
    {
        var ttlInMinutes = int.Parse(configuration.GetSection("Redis")["CartTtlMinutes"]);
        _db = redis.GetDatabase();
        _ttl = TimeSpan.FromMinutes(ttlInMinutes);
        _logger = logger;
    }

    public async Task DeleteAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            await _db.KeyDeleteAsync(CartKey(userId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete cart for user {UserId}", userId);
            throw;
        }
    }

    public async Task<Cart?> GetAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var value = await _db.StringGetAsync(CartKey(userId));
            if (value.IsNullOrEmpty)
            {
                return null;
            }

            // Disambiguate overload by providing a string explicitly
            return JsonConvert.DeserializeObject<Cart>(value.ToString());
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
            var json = System.Text.Json.JsonSerializer.Serialize(cart);
            await _db.StringSetAsync(CartKey(cart.UserId), json, _ttl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save cart for user {UserId}", cart.UserId);
            throw;
        }
    }
}
