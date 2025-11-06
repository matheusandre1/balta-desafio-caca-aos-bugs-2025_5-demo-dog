namespace BugStore.Application.Services.Products.Dto.Request;

public record ProductRequest(string Title, string? Description, string? Slug, decimal Price);
