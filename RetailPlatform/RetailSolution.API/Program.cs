using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RetailPlatform.Carts.Application;
using RetailPlatform.Carts.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.RegisterCartApplication(builder.Configuration);
builder.Services.RegisterCartInfrastructure(builder.Configuration);

var redisConnectionString = builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException("Redis connection string is missing");

builder.Services.AddHealthChecks()
    .AddRedis(
        redisConnectionString,
        name: "redis",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "cache", "ready" });

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("RetailPlatform API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false  // no checks, just confirms the app is running
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = WriteJsonResponse
});

app.UseHttpsRedirection();

app.MapControllers();
app.Run();


static Task WriteJsonResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var payload = new
    {
        status = report.Status.ToString(),
        totalDuration = report.TotalDuration.TotalMilliseconds,
        checks = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            description = e.Value.Description,
            duration = e.Value.Duration.TotalMilliseconds,
            error = e.Value.Exception?.Message
        })
    };

    return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
}