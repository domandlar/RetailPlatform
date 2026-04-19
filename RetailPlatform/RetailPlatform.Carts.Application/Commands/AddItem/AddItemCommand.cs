using MediatR;
using RetailPlatform.Carts.Application.Contracts;
using RetailPlatform.Shared.Primitives.Results;

namespace RetailPlatform.Carts.Application.Commands.AddItem;

public record AddItemCommand(
    string UserId,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
) : IRequest<Result<CartDto>>;
