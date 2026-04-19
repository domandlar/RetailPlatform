namespace RetailPlatform.Carts.Domain.Entities;

public class Cart
{
    public string UserId { get; private set; } = string.Empty;
    public List<CartItem> Items { get; private set; } = [];
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    public int TotalItems => Items.Sum(i => i.Quantity);

    public static Cart Create(
        string userId,
        Guid productId,
        string productName,
        decimal unitPrice, 
        int quantity)
    {
        var cart = new Cart
        {
            UserId = userId
        };
        cart.Items.Add(new CartItem
        {
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity
        });
        return cart;
    }
    public void AddItem(CartItem newItem)
    {
        var existing = Items.FirstOrDefault(i => i.ProductId == newItem.ProductId);
        if (existing is not null)
        {
            existing.Quantity += newItem.Quantity;
        }
        else
        {
            Items.Add(newItem);
        }
        UpdatedAt = DateTime.UtcNow;
    }
}
