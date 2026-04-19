using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetailPlatform.Carts.Application.Services;
using RetailPlatform.Carts.Domain.Services;

namespace RetailPlatform.Carts.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterCartApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICartService, CartService>();
        return services;
    }
}
