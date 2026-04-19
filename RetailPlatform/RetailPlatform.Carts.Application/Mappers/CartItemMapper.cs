using RetailPlatform.Carts.Application.Contracts;
using RetailPlatform.Carts.Domain.Entities;

namespace RetailPlatform.Carts.Application.Mappers;

internal static class CartItemMapper
{
    internal static CartItemDto ToDto(this CartItem item)
    {
        return new CartItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            UnitPrice = item.UnitPrice,
            Quantity = item.Quantity,
            TotalPrice = item.TotalPrice
        };
    }
}
