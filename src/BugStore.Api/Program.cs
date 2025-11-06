using BugStore.Api;
using BugStore.Application;
using BugStore.Application.Services;
using BugStore.Application.Services.Customers.Dto.Request;
using BugStore.Application.Services.Customers.Dto.Response;
using BugStore.Application.Services.Dtos.OrderLines.Request;
using BugStore.Application.Services.Dtos.OrderLines.Response;
using BugStore.Application.Services.Orders.Dto.Request;
using BugStore.Application.Services.Orders.Dto.Response;
using BugStore.Application.Services.Products.Dto.Request;
using BugStore.Application.Services.Products.Dto.Response;
using BugStore.Infrastructure.IoC;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddApplicationModule();
builder.Services.AddValidators();
builder.Services.AddInfraPersistence(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpointss();

app.Run();

[JsonSerializable(typeof(CustomerResponse))]
[JsonSerializable(typeof(CustomerRequest))]
[JsonSerializable(typeof(OrderLineRequest))]
[JsonSerializable(typeof(OrderLineRequest[]))]
[JsonSerializable(typeof(OrderLineResponse))]
[JsonSerializable(typeof(OrderRequest))]
[JsonSerializable(typeof(OrderResponse))]
[JsonSerializable(typeof(ProductResponse))]
[JsonSerializable(typeof(ProductRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}