using BugStore.Application.Services.Interfaces;
using BugStore.Application.Services.Orders.Dto.Request;

namespace BugStore.Api;

public static class OrderEndpoints
{
    public static void MapOrderEndpointss(this IEndpointRouteBuilder routes)
    {
        var orders = routes.MapGroup("v1/orders");

        orders.MapGet("/{id:guid}", async (IOrderService service, Guid id) =>
        {
            var order = await service.GetByIdAsync(id);
            return order is not null ? Results.Ok(order) : Results.NoContent();
        });

        orders.MapPost("/", async (IOrderService service, OrderRequest orderRequest) =>
        {
            await service.CreateAsync(orderRequest);
            return Results.Created($"/v1/orders/{orderRequest}", orderRequest);
        });

    }
}
