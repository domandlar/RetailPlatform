using RetailPlatform.Carts.Domain.Entities;

namespace RetailPlatform.Carts.Domain.Services;
public interface ICartService
{
    Task<Cart?> GetCartAsync(string userId, CancellationToken ct = default);
    Task<Cart> AddItemAsync(
        string userId,
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity,
        CancellationToken ct = default);
    Task<Cart?> UpdateItemAsync(
        string userId, 
        Guid productId, 
        int quantity, 
        CancellationToken ct = default);
    Task<bool> RemoveItemAsync(string userId, Guid productId, CancellationToken ct = default);
    Task<bool> ClearCartAsync(string userId, CancellationToken ct = default);
}
