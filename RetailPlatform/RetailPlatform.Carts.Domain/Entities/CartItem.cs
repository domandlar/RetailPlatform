namespace RetailPlatform.Carts.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal TotalPrice => UnitPrice * Quantity;

    public static CartItem Create(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId must be a non-empty GUID.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("ProductName must be provided.", nameof(productName));

        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "UnitPrice cannot be negative.");

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        return new CartItem
        {
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity
        };
    }
}
