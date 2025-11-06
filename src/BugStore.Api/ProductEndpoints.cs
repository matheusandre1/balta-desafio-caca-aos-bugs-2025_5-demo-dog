using BugStore.Application.Services.Interfaces;
using BugStore.Application.Services.Products.Dto.Request;

namespace BugStore.Api;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes) 
    {
        var products = routes.MapGroup("v1/products");

        products.MapGet("/", async (IProductService service) =>
        {
            var products = await service.GetAllAsync();
            return products is not null ? Results.Ok(products) : Results.NoContent();
        });

        products.MapGet("/{id:guid}", async (IProductService service, Guid id) =>
        {
            var product = await service.GetByIdAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });

        products.MapPost("/", async (IProductService service, ProductRequest productDtoRequest) =>
        {
            await service.CreateAsync(productDtoRequest);
            return Results.Created($"/v1/products/{productDtoRequest}", productDtoRequest);
        });

        products.MapPut("/{id:guid}", async (IProductService service, Guid id, ProductRequest productDtoRequest) =>
        {
            await service.UpdateProductAsync(id, productDtoRequest);
            return Results.NoContent();
        });

        products.MapDelete("/{id:guid}", async (IProductService service, Guid id) =>
        {
            await service.DeleteProductAsync(id);
            return Results.NoContent();
        });
    }
}
