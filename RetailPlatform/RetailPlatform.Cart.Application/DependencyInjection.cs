using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RetailPlatform.Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterCartApplication(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}
