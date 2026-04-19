using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetailPlatform.Carts.Domain.Repositories;
using RetailPlatform.Carts.Infrastructure.Redis;
using StackExchange.Redis;

namespace RetailPlatform.Carts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterCartInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Redis")["ConnectionString"]
            ?? throw new InvalidOperationException("Redis ConnectionString is missing.");


        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(connectionString));
        services.AddScoped<ICartRepository, RedisCartRepository>();
        return services;
    }
}
