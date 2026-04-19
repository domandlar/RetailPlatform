using MediatR;
using RetailPlatform.Carts.Application.Contracts;
using RetailPlatform.Carts.Application.Mappers;
using RetailPlatform.Carts.Domain.Entities;
using RetailPlatform.Carts.Domain.Repositories;
using RetailPlatform.Shared.Primitives.Results;

namespace RetailPlatform.Carts.Application.Commands.AddItem;

public class AddItemHandler : IRequestHandler<AddItemCommand, Result<CartDto>>
{
    private readonly ICartRepository _repository;

    public AddItemHandler(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CartDto>> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
            var cart = await _repository.GetAsync(request.UserId, cancellationToken)
                ?? Cart.Create(
                    request.UserId,
                    request.ProductId,
                    request.ProductName,
                    request.UnitPrice,
                    request.Quantity);

            var newItem = CartItem.Create(
                request.ProductId,
                request.ProductName,
                request.UnitPrice,
                request.Quantity);
            cart.AddItem(newItem);

        await _repository.SaveAsync(cart);

        return cart.ToDto();
    }
}
