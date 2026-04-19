using Microsoft.AspNetCore.Mvc;
using RetailPlatform.Carts.Application.Contracts;
using RetailPlatform.Carts.Domain.Services;

namespace RetailPlatform.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId, CancellationToken ct)
    {
        var cart = await _cartService.GetCartAsync(userId, ct);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpPost("{userId}/items")]
    public async Task<IActionResult> AddItem(string userId, AddItemRequest request, CancellationToken ct)
    {
        var cart = await _cartService.AddItemAsync(
            userId,
            request.ProductId,
            request.ProductName,
            request.UnitPrice,
            request.Quantity,
            ct);
        return Ok(cart);
    }

    [HttpPut("{userId}/items/{productId}")]
    public async Task<IActionResult> UpdateItem(string userId, Guid productId, UpdateItemRequest request, CancellationToken ct)
    {
        var cart = await _cartService.UpdateItemAsync(userId, productId, request.Quantity, ct);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpDelete("{userId}/items/{productId}")]
    public async Task<IActionResult> RemoveItem(string userId, Guid productId, CancellationToken ct)
    {
        var removed = await _cartService.RemoveItemAsync(userId, productId, ct);
        return removed ? NoContent() : NotFound();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(string userId, CancellationToken ct)
    {
        var cleared = await _cartService.ClearCartAsync(userId, ct);
        return cleared ? NoContent() : NotFound();
    }
}