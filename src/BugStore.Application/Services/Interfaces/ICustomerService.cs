using BugStore.Application.Services.Customers.Dto.Request;
using BugStore.Application.Services.Customers.Dto.Response;

namespace BugStore.Application.Services.Interfaces;
public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetAllAsync();
    Task<CustomerResponse> GetByIdAsync(Guid id);
    Task CreateAsync(CustomerRequest customerRequest);
    Task<CustomerResponse> UpdateCustomerAsync(Guid id, CustomerRequest dto);
    Task DeleteCustomerAsync(Guid id);
}
