namespace BugStore.Api;
using global::BugStore.Application.Services.Customers.Dto.Request;
using global::BugStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var customers = routes.MapGroup("v1/customers");

        customers.MapGet("/", async (ICustomerService service) =>
        {
            var customers = await service.GetAllAsync();

            return customers is not null ? Results.Ok(customers) : Results.NotFound();

        });

        customers.MapGet("/{id:guid}", async (ICustomerService service, Guid id) =>
        {
            var customer = await service.GetByIdAsync(id);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        });

        customers.MapPost("/", async (ICustomerService service, CustomerRequest customerDtoRequest) =>
        {
            await service.CreateAsync(customerDtoRequest);
            return Results.Created($"/v1/customers/{customerDtoRequest}", customerDtoRequest);
        });

        customers.MapPut("/{id:guid}", async (ICustomerService service, Guid id, CustomerRequest customerDtoRequest) =>
        {
            var updatedCustomer = await service.UpdateCustomerAsync(id, customerDtoRequest);
            return updatedCustomer is not null ? Results.Ok(updatedCustomer) : Results.NotFound();
        });

        customers.MapDelete("/{id:guid}", async (ICustomerService service, Guid id) =>
        {
            await service.DeleteCustomerAsync(id);
            return Results.NoContent();
        });
    }    
}