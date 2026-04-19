using MediatR;
using RetailPlatform.Carts.Application;
using RetailPlatform.Carts.Application.Commands.AddItem;
using RetailPlatform.Carts.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterCartApplication(builder.Configuration);
builder.Services.RegisterCartInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("RetailPlatform API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});


app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

var cart = app.MapGroup("/api/cart").WithTags("Cart");
// POST /api/cart/{userId}/items
cart.MapPost("/{userId}/items", async (
    string userId,
    AddItemRequest request,
    IMediator mediator
    //,
    //IValidator<AddItemCommand> validator
    ) =>
{
    var command = new AddItemCommand(
        userId,
        request.ProductId,
        request.ProductName,
        request.UnitPrice,
        request.Quantity);

    //var validation = await validator.ValidateAsync(command);
    //if (!validation.IsValid)
    //    return Results.ValidationProblem(validation.ToDictionary());

    var result = await mediator.Send(command);
    return Results.Ok(result);
})
.WithName("AddItem")
.Produces(200)
.Produces(400);


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record AddItemRequest(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);
