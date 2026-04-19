using RetailPlatform.Carts.Domain.Entities;

namespace RetailPlatform.Carts.Application.Contracts;

public class CartDto
{
    public string UserId { get; init; }
    public List<CartItemDto> Items { get; init; } = [];
    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; init; } 
    public decimal TotalPrice { get; init; }
    public int TotalItems { get; init; }
}
