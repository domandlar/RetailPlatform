using RetailPlatform.Carts.Application.Contracts;
using RetailPlatform.Carts.Domain.Entities;

namespace RetailPlatform.Carts.Application.Mappers;

internal static class CartMapper
{
    internal static CartDto ToDto(this Cart cart)
    {
        return new CartDto
        {
            UserId = cart.UserId,
            Items = cart.Items
                .Select(x=>x.ToDto())
                .ToList(),
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            TotalPrice = cart.TotalPrice,
            TotalItems = cart.TotalItems
        };
    }
}
