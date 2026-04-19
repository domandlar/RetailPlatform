using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RetailPlatform.Carts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterCartInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}
