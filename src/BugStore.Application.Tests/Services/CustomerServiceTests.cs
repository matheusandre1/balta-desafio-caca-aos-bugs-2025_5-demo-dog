using AutoFixture;
using AutoMapper;
using BugStore.Application.Services.Customers.Dto.Request;
using BugStore.Application.Services.Customers.Dto.Response;
using BugStore.Application.Services.Customers.Services;
using BugStore.Application.Services.Interfaces;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Services;
public class CustomerServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Customer>> _customerRepositoryMock;
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerServiceTests()
    {
        _fixture = new Fixture();
        _customerRepositoryMock = new Mock<IRepository<Customer>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CustomerRequest, Customer>();
            cfg.CreateMap<Customer, CustomerResponse>();
        });
        _mapper = config.CreateMapper();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomer()
    {
        var request = _fixture.Create<CustomerRequest>();

        _customerRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                 .Returns(Task.CompletedTask);

        await _customerService.CreateAsync(request);

        _customerRepositoryMock
            .Verify(r => r.AddAsync(It.IsAny<Customer>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenCustomerNotFound()
    {

        var id = _fixture.Create<Guid>();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Customer)null);


        var act = () => _customerService.GetByIdAsync(id);


        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("Customer not found");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {

        var id = Guid.NewGuid();
        var customer = _fixture.Build<Customer>()
                              .With(c => c.Id, id)
                              .With(c => c.BirthDate, _fixture.Create<DateTime>())
                              .Create();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(customer);


        var result = await _customerService.GetByIdAsync(id);


        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        _customerRepositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
   

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCustomers()
    {
        var customers = _fixture.Build<Customer>()
            .With(c => c.BirthDate, _fixture.Create<DateTime>())
            .CreateMany(2)
            .ToList();

        _customerRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        var result = await _customerService.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(customers.Count);
        result.Should().BeEquivalentTo(customers.Select(c => _mapper.Map<CustomerResponse>(c)));

        _customerRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);

    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldThrowException_WhenCustomerNotFound()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Build<CustomerRequest>()
            .With(r => r.BirthDate, _fixture.Create<DateTime>())
            .Create();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer)null);

        var act = () => _customerService.UpdateCustomerAsync(id, request);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("Customer not found");

        _customerRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);

    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldUpdateAndReturnResponse()
    {
        var id = _fixture.Create<Guid>();
        var existing = _fixture.Build<Customer>()
            .With(c => c.Id, id)
            .With(c => c.BirthDate, _fixture.Create<DateTime>())
            .Create();

        var request = _fixture.Build<CustomerRequest>()
            .With(r => r.BirthDate, _fixture.Create<DateTime>())
            .Create();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _customerRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        var result = await _customerService.UpdateCustomerAsync(id, request);

        result.Id.Should().Be(id);
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
        result.Phone.Should().Be(request.Phone);
        result.BirthDate.Should().Be(request.BirthDate!.Value);

        _customerRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Customer>(c =>
            c.Id == id &&
            c.Name == request.Name &&
            c.Email == request.Email &&
            c.Phone == request.Phone &&
            c.BirthDate == request.BirthDate)), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldThrowException_WhenCustomerNotFound()
    {
        var id = Guid.NewGuid();
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Customer)null);

        var act = () => _customerService.DeleteCustomerAsync(id);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Customer not found");

        _customerRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Never);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldDelete_WhenCustomerFound()
    {
        var id = _fixture.Create<Guid>();
        var existing = _fixture.Build<Customer>()
            .With(c => c.Id, id)
            .Create();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _customerRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        await _customerService.DeleteCustomerAsync(id);

        _customerRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}