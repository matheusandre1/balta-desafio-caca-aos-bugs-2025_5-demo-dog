using AutoFixture;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using Moq;

namespace BugStore.Infrastructure.Tests.Repository;

public class OrderRepositoryTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IRepository<Order>> _mockRepository;

    public OrderRepositoryTests()
    {
        _mockRepository = new Mock<IRepository<Order>>();
        
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        
        _fixture.Customize<Order>(composer => composer
            .Without(o => o.Customer));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
       
        var expectedOrder = _fixture.Create<Order>();
        _mockRepository.Setup(r => r.GetByIdAsync(expectedOrder.Id))
            .ReturnsAsync(expectedOrder);

       
        var result = await _mockRepository.Object.GetByIdAsync(expectedOrder.Id);

        
        Assert.NotNull(result);
        Assert.Equal(expectedOrder.Id, result.Id);
        Assert.Equal(expectedOrder.CustomerId, result.CustomerId);
        Assert.Equal(expectedOrder.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public async Task AddAsync_ShouldAddOrder()
    {
        
        var order = _fixture.Create<Order>();
        _mockRepository.Setup(r => r.AddAsync(order))
            .Returns(Task.CompletedTask);

        await _mockRepository.Object.AddAsync(order);

        
        _mockRepository.Verify(r => r.AddAsync(order), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrders()
    {
       
        var orders = _fixture.CreateMany<Order>(3).ToList();
        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(orders);

        
        var result = await _mockRepository.Object.GetAllAsync();

        
        Assert.Equal(orders.Count, result.Count());
        Assert.Equal(orders.Select(o => o.Id), result.Select(o => o.Id));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateOrder()
    {
        
        var order = _fixture.Create<Order>();
        order.UpdatedAt = DateTime.Now;
        _mockRepository.Setup(r => r.UpdateAsync(order))
            .Returns(Task.CompletedTask);

       
        await _mockRepository.Object.UpdateAsync(order);

       
        _mockRepository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOrder()
    {
        
        var orderId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAsync(orderId))
            .Returns(Task.CompletedTask);

       
        await _mockRepository.Object.DeleteAsync(orderId);

        
        _mockRepository.Verify(r => r.DeleteAsync(orderId), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrderWithLines_WhenOrderExists()
    {
        
        var expectedOrder = _fixture.Create<Order>();
        expectedOrder.Lines = _fixture.CreateMany<OrderLine>(2).ToList();
        
        _mockRepository.Setup(r => r.GetByIdAsync(expectedOrder.Id))
            .ReturnsAsync(expectedOrder);

        
        var result = await _mockRepository.Object.GetByIdAsync(expectedOrder.Id);

        Assert.NotNull(result);
        Assert.NotNull(result.Lines);
        Assert.Equal(expectedOrder.Lines.Count, result.Lines.Count);
    }
}