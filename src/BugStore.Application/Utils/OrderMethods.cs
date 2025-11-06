using AutoMapper;
using BugStore.Application.Services.Customers.Dto.Response;
using BugStore.Application.Services.Dtos.OrderLines.Request;
using BugStore.Application.Services.Dtos.OrderLines.Response;
using BugStore.Application.Services.Orders.Dto.Request;
using BugStore.Application.Services.Orders.Dto.Response;
using BugStore.Application.Services.Products.Dto.Response;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;

namespace BugStore.Application.Utils;
public class OrderMethods
{
    public static void ValidateId(Guid CustomerId)
    {
        if (CustomerId == Guid.Empty)
        {
            throw new ArgumentException("CustomerId Invalido");
        }
    }

    public static void ValidateLines(List<OrderLineRequest> lines)
    {
        if (lines is null || lines.Count == 0)
        {
            throw new ArgumentException("Pedido deve conter ao menos um linha");
        }

        foreach (var line in lines)
        {
            if (line.ProductId == Guid.Empty)
            {
                throw new ArgumentException("ProductId Invalido na linha do pedido");
            }

            if (line.Quantity <= 0)
            {
                throw new ArgumentException("Quantidade deve ser maior que zero na linha do pedido");
            }
        }
    }
    public static void Validate(OrderRequest orderRequest)
    {

        ValidateId(orderRequest.CustomerId);
        ValidateLines(orderRequest.Lines);
    }
    public static Order InicializeOrder(OrderRequest orderRequest, IMapper mapper)
    {
        var order = mapper.Map<Order>(orderRequest);

        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        order.Lines = new List<OrderLine>();

        order.Lines.Clear();

        foreach (var lineDto in orderRequest.Lines)
        {
            order.Lines.Add(new OrderLine
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = lineDto.ProductId,
                Quantity = lineDto.Quantity,
                Total = 0m
            });
        }
        return order;
    }

    public static async Task ComputeTotalAsync(Order order, IRepository<Product> productRepository)
    {
        foreach (var line in order.Lines)
        {
            var product = await productRepository.GetByIdAsync(line.ProductId);
            if (product is null)
            {
                throw new ArgumentException($"Produto com Id {line.ProductId} nao encontrado");
            }
            line.Total = product.Price * line.Quantity;
        }
    }

    public static void EnsureLineIds(Order order)
    {
        foreach (var line in order.Lines)
        {
            if (line.Id == Guid.Empty)
            {
                line.Id = Guid.NewGuid();
            }
        }
    }

    public static void AttachOrderIdToLines(Order order)
    {
        foreach (var line in order.Lines)
        {
            line.OrderId = order.Id;
        }
    }

    public static async Task<OrderResponse> BuildDtoAsync(Order order, IRepository<Product> productRepository, IMapper mapper)
    {
        var linesDto = new List<OrderLineResponse>();

        foreach (var line in order.Lines)
        {
            var product = await productRepository.GetByIdAsync(line.ProductId);

            var productDto = mapper.Map<ProductResponse>(product);
            var lineDto = mapper.Map<OrderLineResponse>(line);

            linesDto.Add(new OrderLineResponse(lineDto.Id, lineDto.OrderId, lineDto.Quantity, lineDto.Total, lineDto.ProductId, productDto));

        }

        var customerDto = mapper.Map<CustomerResponse>(order.Customer);

        return new OrderResponse(order.Id, order.CustomerId, customerDto!, order.CreatedAt, order.UpdatedAt, linesDto);
    }
}
