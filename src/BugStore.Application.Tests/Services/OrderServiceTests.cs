using AutoFixture;
using AutoMapper;
using BugStore.Application.Services.Dtos.OrderLines.Request;
using BugStore.Application.Services.Interfaces;
using BugStore.Application.Services.Orders.Dto.Request;
using BugStore.Application.Services.Orders.Services;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Services;
public class OrderServiceTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly IOrderService _orderService;

    public OrderServiceTests()
    {
        _fixture = new Fixture();
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _productRepositoryMock = new Mock<IRepository<Product>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<OrderRequest, Order>();
            cfg.CreateMap<OrderLineRequest, OrderLine>();
        });
        _mapper = config.CreateMapper();

        _orderService = new OrderService(_orderRepositoryMock.Object, _productRepositoryMock.Object, _mapper);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    
    public async Task CreateAsync_ShouldThrowException_WhenProductNotFound()
    {
        var request = _fixture.Create<OrderRequest>();
        var product = _fixture.Create<Product>();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(request.Lines.First().ProductId))
            .ReturnsAsync((Product)null);

        var act = () => _orderService.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Produto com Id {request.Lines.First().ProductId} nao encontrado");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenOrderNotFound()
    {
        var orderId = _fixture.Create<Guid>();

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync((Order)null);

        var act = () => _orderService.GetByIdAsync(orderId);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Order Not Found");
    }
}
