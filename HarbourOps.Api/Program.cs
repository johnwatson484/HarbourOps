using System.ComponentModel.DataAnnotations;
using HarbourOps.Adapters.EfSqlite;
using HarbourOps.Adapters.EfSqlite.Persistence;
using HarbourOps.Adapters.EmailConsole;
using HarbourOps.Adapters.FakePayments;
using HarbourOps.Application;
using HarbourOps.Application.Bookings;
using HarbourOps.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddEfSqliteAdapter(builder.Configuration)
    .AddFakePaymentAdapter()
    .AddConsoleEmailAdapter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => Results.Redirect("/openapi/v1.json"));

var services = app.MapGroup("/services")
    .WithTags("Services");

services.MapGet("/", async (
    ListServicesHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(cancellationToken);
    return Results.Ok(result);
});

var bookings = app.MapGroup("/bookings")
    .WithTags("Bookings");

bookings.MapGet("/", async (
    ListRecentBookingsHandler handler,
    CancellationToken cancellationToken,
    int count = 20) =>
{
    var result = await handler.HandleAsync(count, cancellationToken);
    return Results.Ok(result);
});

bookings.MapGet("/{id:guid}", async (
    Guid id,
    GetBookingHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(id, cancellationToken);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

bookings.MapPost("/", async (
    CreateBookingRequest request,
    CreateBookingHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(
        new CreateBookingCommand(
            request.CustomerEmail,
            request.VesselName,
            request.ContainerNumber,
            request.RequestedDate),
        cancellationToken);

    return Results.Created($"/bookings/{result.Id}", result);
});

bookings.MapPost("/{id:guid}/services", async (
    Guid id,
    AddServiceRequest request,
    AddServiceToBookingHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(
        new AddServiceToBookingCommand(id, request.ServiceItemId, request.Quantity),
        cancellationToken);

    return result is null ? Results.NotFound() : Results.Ok(result);
});

bookings.MapPost("/{id:guid}/submit", async (
    Guid id,
    SubmitBookingHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(id, cancellationToken);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

bookings.MapPost("/{id:guid}/checkout", async (
    Guid id,
    CheckoutBookingHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(id, cancellationToken);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

bookings.MapPost("/{id:guid}/fulfil", async (
    Guid id,
    FulfilBookingHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.HandleAsync(id, cancellationToken);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HarbourOpsDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();

public sealed record CreateBookingRequest(
    [property: Required, EmailAddress] string CustomerEmail,
    [property: Required, MinLength(2)] string VesselName,
    [property: Required, MinLength(4)] string ContainerNumber,
    DateOnly RequestedDate);

public sealed record AddServiceRequest(
    Guid ServiceItemId,
    [property: Range(1, 100)] int Quantity);
