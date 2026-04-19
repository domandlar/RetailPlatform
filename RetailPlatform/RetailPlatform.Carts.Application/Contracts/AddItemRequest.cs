namespace RetailPlatform.Carts.Application.Contracts;

public record AddItemRequest(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);
