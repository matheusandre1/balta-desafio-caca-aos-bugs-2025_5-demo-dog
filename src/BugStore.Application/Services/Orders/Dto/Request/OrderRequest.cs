using BugStore.Application.Services.Dtos.OrderLines.Request;

namespace BugStore.Application.Services.Orders.Dto.Request;

public record OrderRequest(Guid CustomerId,List<OrderLineRequest> Lines);
