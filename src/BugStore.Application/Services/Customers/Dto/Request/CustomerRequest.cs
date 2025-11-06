namespace BugStore.Application.Services.Customers.Dto.Request;

public record CustomerRequest(string? Name, string? Email, string? Phone, DateTime? BirthDate);
