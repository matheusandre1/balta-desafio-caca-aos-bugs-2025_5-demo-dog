using BugStore.Application.Services.Products.Dto.Request;
using BugStore.Domain.Entities;

namespace BugStore.Application.Utils;
public static class ProductMethods
{
    public static Product CreateProduct(ProductRequest request)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        };        
    }    
}
