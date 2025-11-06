namespace BugStore.Application.Services.Products.Dto.Response;

public record ProductResponse(Guid Id, string Title, string? Description, string? Slug, decimal Price);