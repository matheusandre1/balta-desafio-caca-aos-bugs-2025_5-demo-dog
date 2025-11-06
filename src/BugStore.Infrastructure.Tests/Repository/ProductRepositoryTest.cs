using AutoFixture;
using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using Moq;

namespace BugStore.Infrastructure.Tests.Repository;
public class ProductRepositoryTest
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IRepository<Product>> _mockRepository;

    public ProductRepositoryTest()
    {
        _mockRepository = new Mock<IRepository<Product>>();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {

        var expectedProduct = _fixture.Create<Product>();
        _mockRepository.Setup(r => r.GetByIdAsync(expectedProduct.Id))
            .ReturnsAsync(expectedProduct);


        var result = await _mockRepository.Object.GetByIdAsync(expectedProduct.Id);


        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Title, result.Title);
        Assert.Equal(expectedProduct.Description, result.Description);
        Assert.Equal(expectedProduct.Slug, result.Slug);
        Assert.Equal(expectedProduct.Price, result.Price);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {

        var product = _fixture.Create<Product>();
        _mockRepository.Setup(r => r.AddAsync(product))
            .Returns(Task.CompletedTask);


        await _mockRepository.Object.AddAsync(product);


        _mockRepository.Verify(r => r.AddAsync(product), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {

        var products = _fixture.CreateMany<Product>(3).ToList();
        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);


        var result = await _mockRepository.Object.GetAllAsync();

        Assert.Equal(products.Count, result.Count());
        Assert.Equal(products.Select(c => c.Id), result.Select(c => c.Id));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomer()
    {

        var product = _fixture.Create<Product>();
        product.Title = "Updated Title";
        _mockRepository.Setup(r => r.UpdateAsync(product))
            .Returns(Task.CompletedTask);


        await _mockRepository.Object.UpdateAsync(product);


        _mockRepository.Verify(r => r.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduct()
    {

        var productId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAsync(productId))
            .Returns(Task.CompletedTask);


        await _mockRepository.Object.DeleteAsync(productId);


        _mockRepository.Verify(r => r.DeleteAsync(productId), Times.Once);
    }
}
