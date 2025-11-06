using BugStore.Application.Services.Products.Dto.Response;

namespace BugStore.Application.Services.Dtos.OrderLines.Response;
public record OrderLineResponse(Guid Id, Guid OrderId, int Quantity, decimal Total, Guid ProductId, ProductResponse Product);

