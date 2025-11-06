using AutoFixture;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using Moq;

namespace BugStore.Infrastructure.Tests.Repository;

public class RepositoryCustomerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IRepository<Customer>> _mockRepository;

    public RepositoryCustomerTests()
    {
        _mockRepository = new Mock<IRepository<Customer>>();
        
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
       
        var expectedCustomer = _fixture.Create<Customer>();
        _mockRepository.Setup(r => r.GetByIdAsync(expectedCustomer.Id))
            .ReturnsAsync(expectedCustomer);

        
        var result = await _mockRepository.Object.GetByIdAsync(expectedCustomer.Id);

        
        Assert.NotNull(result);
        Assert.Equal(expectedCustomer.Id, result.Id);
        Assert.Equal(expectedCustomer.Name, result.Name);
        Assert.Equal(expectedCustomer.Email, result.Email);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCustomer()
    {
        
        var customer = _fixture.Create<Customer>();
        _mockRepository.Setup(r => r.AddAsync(customer))
            .Returns(Task.CompletedTask);

        
        await _mockRepository.Object.AddAsync(customer);

        
        _mockRepository.Verify(r => r.AddAsync(customer), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        
        var customers = _fixture.CreateMany<Customer>(3).ToList();
        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(customers);

        
        var result = await _mockRepository.Object.GetAllAsync();

        Assert.Equal(customers.Count, result.Count());
        Assert.Equal(customers.Select(c => c.Id), result.Select(c => c.Id));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomer()
    {
        
        var customer = _fixture.Create<Customer>();
        customer.Name = "Updated Name";
        _mockRepository.Setup(r => r.UpdateAsync(customer))
            .Returns(Task.CompletedTask);

        
        await _mockRepository.Object.UpdateAsync(customer);

        
        _mockRepository.Verify(r => r.UpdateAsync(customer), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomer()
    {
        
        var customerId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAsync(customerId))
            .Returns(Task.CompletedTask);

        
        await _mockRepository.Object.DeleteAsync(customerId);

        
        _mockRepository.Verify(r => r.DeleteAsync(customerId), Times.Once);
    }
}
