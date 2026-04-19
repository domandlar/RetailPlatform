namespace RetailPlatform.Carts.Application.Contracts;

public class CartItemDto
{
    public Guid ProductId { get; init; } 
    public string ProductName { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
}
