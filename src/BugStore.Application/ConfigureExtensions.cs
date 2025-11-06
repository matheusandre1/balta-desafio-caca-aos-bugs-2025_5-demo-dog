using BugStore.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BugStore.Application;
public static class ConfigureExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        
        services.AddValidatorsFromAssemblyContaining<CustomerRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductRequestValidators>();        
        services.AddValidatorsFromAssemblyContaining<OrderLineRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<OrderRequestValidators>();
        return services;
    }

}
