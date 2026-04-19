using FluentAssertions;
using RetailPlatform.Carts.Domain.Entities;

namespace RetailPlatform.Carts.Domain.Tests.Entities;

public class CartTests
{
    private const string UserId = "user-123";

    private static readonly Guid ProductA = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ProductB = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public void GivenValidInputs_WhenCreatingCart_ThenUserIdAndInitialItemAreSet()
    {
        // Given
        const string productName = "Widget";
        const decimal unitPrice = 19.99m;
        const int quantity = 2;

        // When
        var cart = Cart.Create(UserId, ProductA, productName, unitPrice, quantity);

        // Then
        cart.UserId.Should().Be(UserId);
        cart.Items.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                ProductId = ProductA,
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity
            });
    }

    [Fact]
    public void GivenCartWithoutProduct_WhenAddingItem_ThenItemIsAppended()
    {
        // Given
        var cart = Cart.Create(UserId, ProductA, "Widget", 10m, 1);
        var newItem = new CartItem
        {
            ProductId = ProductB,
            ProductName = "Gadget",
            UnitPrice = 5m,
            Quantity = 3
        };

        // When
        cart.AddItem(newItem);

        // Then
        cart.Items.Should().HaveCount(2);
        cart.Items.Should().ContainSingle(i => i.ProductId == ProductB)
            .Which.Quantity.Should().Be(3);
    }

    [Fact]
    public void GivenCartContainingProduct_WhenAddingSameProduct_ThenQuantitiesAreMerged()
    {
        // Given
        var cart = Cart.Create(UserId, ProductA, "Widget", 10m, 2);
        var duplicate = new CartItem
        {
            ProductId = ProductA,
            ProductName = "Widget",
            UnitPrice = 10m,
            Quantity = 3
        };

        // When
        cart.AddItem(duplicate);

        // Then
        cart.Items.Should().ContainSingle();
        cart.Items.Single().Quantity.Should().Be(5);
    }

    [Fact]
    public void GivenCartWithMultipleItems_WhenCalculatingTotal_ThenSumsPriceTimesQuantity()
    {
        // Given
        var cart = Cart.Create(UserId, ProductA, "Widget", 10m, 2);    // 20.00
        cart.AddItem(new CartItem
        {
            ProductId = ProductB,
            ProductName = "Gadget",
            UnitPrice = 4.5m,
            Quantity = 3                                                // 13.50
        });

        // When
        var total = cart.TotalPrice;

        // Then
        total.Should().Be(33.5m);
    }
}