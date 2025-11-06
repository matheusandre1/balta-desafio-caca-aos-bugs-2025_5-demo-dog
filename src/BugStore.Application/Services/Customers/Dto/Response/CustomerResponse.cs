namespace BugStore.Application.Services.Customers.Dto.Response;

public record CustomerResponse(Guid Id, string Name, string Email, string Phone, DateTime BirthDate);