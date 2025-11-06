using BugStore.Application.Services.Products.Dto.Request;
using BugStore.Application.Services.Products.Dto.Response;

namespace BugStore.Application.Services.Interfaces;
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task CreateAsync(ProductRequest customerRequest);
    Task UpdateProductAsync(Guid id, ProductRequest dto);
    Task DeleteProductAsync(Guid id);
}
