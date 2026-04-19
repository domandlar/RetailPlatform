using RetailPlatform.Carts.Domain.Entities;
using RetailPlatform.Carts.Domain.Repositories;

namespace RetailPlatform.Carts.Application.Services;

public interface ICartService
{
    Task<Cart?> GetCartAsync(string userId, CancellationToken ct = default);
    Task<Cart> AddItemAsync(string userId, AddItemRequest request, CancellationToken ct = default);
    Task<Cart?> UpdateItemAsync(string userId, Guid productId, int quantity, CancellationToken ct = default);
    Task<bool> RemoveItemAsync(string userId, Guid productId, CancellationToken ct = default);
    Task<bool> ClearCartAsync(string userId, CancellationToken ct = default);
}
public record AddItemRequest(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);

public record UpdateItemRequest(int Quantity);


public class CartService : ICartService
{
    private readonly ICartRepository _repository;

    public CartService(ICartRepository repo)
    {
        _repository = repo;
    }

    public Task<Cart?> GetCartAsync(string userId, CancellationToken ct = default)
        => _repository.GetAsync(userId, ct);

    public async Task<Cart> AddItemAsync(string userId, AddItemRequest request, CancellationToken ct = default)
    {
        var cart = await _repository.GetAsync(userId, ct);

        if (cart is null)
        {
            cart = Cart.Create(
                userId,
                request.ProductId,
                request.ProductName,
                request.UnitPrice,
                request.Quantity);
        }
        else
        {
            cart.AddItem(new CartItem
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                UnitPrice = request.UnitPrice,
                Quantity = request.Quantity
            });
        }

        await _repository.SaveAsync(cart, ct);
        return cart;
    }

    public async Task<Cart?> UpdateItemAsync(string userId, Guid productId, int quantity, CancellationToken ct = default)
    {
        var cart = await _repository.GetAsync(userId, ct);
        if (cart is null) return null;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return null;

        if (quantity <= 0)
            cart.Items.Remove(item);
        else
            item.Quantity = quantity;

        await _repository.SaveAsync(cart, ct);
        return cart;
    }

    public async Task<bool> RemoveItemAsync(string userId, Guid productId, CancellationToken ct = default)
    {
        var cart = await _repository.GetAsync(userId, ct);
        if (cart is null) return false;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return false;

        cart.Items.Remove(item);
        await _repository.SaveAsync(cart, ct);
        return true;
    }

    public async Task<bool> ClearCartAsync(string userId, CancellationToken ct = default)
    {
        var cart = await _repository.GetAsync(userId, ct);
        if (cart is null) return false;

        await _repository.DeleteAsync(userId, ct);
        return true;
    }
}