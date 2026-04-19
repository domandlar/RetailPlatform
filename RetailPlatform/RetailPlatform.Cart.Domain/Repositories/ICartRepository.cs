using RetailPlatform.Carts.Domain.Entities;

namespace RetailPlatform.Carts.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetAsync(string userId, CancellationToken ct = default);
    Task SaveAsync(Cart cart, CancellationToken ct = default);
    Task DeleteAsync(string userId, CancellationToken ct = default);
}
