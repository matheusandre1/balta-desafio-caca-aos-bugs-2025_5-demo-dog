using AutoFixture;
using AutoMapper;
using BugStore.Application.Services.Interfaces;
using BugStore.Application.Services.Products.Dto.Request;
using BugStore.Application.Services.Products.Dto.Response;
using BugStore.Application.Services.Products.Services;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Services;
public class ProductServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductServiceTests()
    {
        _fixture = new Fixture();
        _productRepositoryMock = new Mock<IRepository<Product>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductRequest, Product>();
            cfg.CreateMap<Product, ProductResponse>();
        });
        _mapper = config.CreateMapper();
        _productService = new ProductService
            (_productRepositoryMock.Object, _mapper);
    }

    [Fact]
    
    public async Task CreateAsync_ShouldAddProduct()
    {
        var request = _fixture.Create<ProductRequest>();

        _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                 .Returns(Task.CompletedTask);

        await _productService.CreateAsync(request);

        _productRepositoryMock
            .Verify(r => r.AddAsync(It.IsAny<Product>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenProductNotFound()
    {

        var id = _fixture.Create<Guid>();

        _productRepositoryMock?.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Product?)null);


        var act = () => _productService.GetByIdAsync(id);


        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("Product Not Found");
    }

    [Fact]    
    public async Task GetAllAsync_ShouldReturnMappedProducts()
    {
        var customers = _fixture.Build<Product>()
            .CreateMany(2)
            .ToList();

        _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        var result = await _productService.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(customers.Count);
        result.Should().BeEquivalentTo(customers.Select(c => _mapper.Map<ProductResponse>(c)));

        _productRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrowException_WhenProductNotFound()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Build<ProductRequest>()
            .Create();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);

        var act = () => _productService.UpdateProductAsync(id, request);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("Product Not Found");

        _productRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);

    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateAndReturnResponse()
    {
        var id = _fixture.Create<Guid>();

        var existing = _fixture.Build<Product>()
            .With(p => p.Id, id)
            .Create();

        var request = _fixture.Build<ProductRequest>()
            .Create();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _productRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        await _productService.UpdateProductAsync(id, request);

        _productRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Product>(c =>
            c.Id == id &&
            c.Title == request.Title &&
            c.Description == request.Description &&
            c.Price == request.Price
        )), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldThrowException_WhenProductNotFound()
    {
        var id = Guid.NewGuid();
        _productRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Product)null);

        var act = () => _productService.DeleteProductAsync(id);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Product Not Found");

        _productRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Never);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldDelete_WhenProductFound()
    {
        var id = _fixture.Create<Guid>();
        var existing = _fixture.Build<Product>()
            .With(c => c.Id, id)
            .Create();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _productRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        await _productService.DeleteProductAsync(id);

        _productRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}
