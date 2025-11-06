using BugStore.Application.Services.Customers.Dto.Response;
using BugStore.Application.Services.Dtos.OrderLines.Response;

namespace BugStore.Application.Services.Orders.Dto.Response;

public record OrderResponse(Guid Id, Guid CustomerId, CustomerResponse Customer, DateTime CreatedAt, DateTime UpdatedAt, List<OrderLineResponse> Lines);