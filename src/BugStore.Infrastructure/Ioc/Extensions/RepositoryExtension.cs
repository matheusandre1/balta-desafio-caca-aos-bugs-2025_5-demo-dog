using BugStore.Domain.Base;
using BugStore.Infrastructure.Context;
using BugStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace BugStore.Infrastructure.IoC;

public static class InfraServiceCollectionExtensions
{
    public static IServiceCollection AddInfraPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = "Data Source=bugstore.db";
        
        if (configuration != null)
        {
            var connStr = configuration.GetSection("ConnectionStrings").GetSection("Default").Value;
            if (!string.IsNullOrEmpty(connStr))
            {
                connectionString = connStr;
            }
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}