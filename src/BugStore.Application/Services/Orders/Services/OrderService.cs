using AutoMapper;
using BugStore.Application.Services.Interfaces;
using BugStore.Application.Services.Orders.Dto.Request;
using BugStore.Application.Services.Orders.Dto.Response;
using BugStore.Application.Utils;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;

namespace BugStore.Application.Services.Orders.Services;
public class OrderService(IRepository<Order> _orderRepository, IRepository<Product> _productRepository, IMapper _mapper) : IOrderService
{
    public async Task<OrderResponse> GetByIdAsync(Guid id)
    {
        var entity = await  _orderRepository.GetByIdAsync(id);

        if(entity is null) throw new Exception("Order Not Found");

        return await OrderMethods.BuildDtoAsync(entity, _productRepository, _mapper);
      
    }
    public async Task CreateAsync(OrderRequest orderRequest)
    {
        OrderMethods.Validate(orderRequest);

        var order = OrderMethods.InicializeOrder(orderRequest, _mapper);

        await OrderMethods.ComputeTotalAsync(order, _productRepository);

        OrderMethods.EnsureLineIds(order);
        OrderMethods.AttachOrderIdToLines(order);
        
        await _orderRepository.AddAsync(order);        
    }       
}
