using BugStore.Domain.Base;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Context;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BugStore.Infrastructure.Tests.Ioc;

public class InfraRegisterTests
{
    private readonly IServiceCollection _services;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IConfigurationSection> _mockConnectionStringsSection;
    private readonly Mock<IConfigurationSection> _mockDefaultSection;

    public InfraRegisterTests()
    {
        _services = new ServiceCollection();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConnectionStringsSection = new Mock<IConfigurationSection>();
        _mockDefaultSection = new Mock<IConfigurationSection>();        
        
        _mockDefaultSection.Setup(s => s.Value).Returns("Data Source=test.db");
        _mockConnectionStringsSection.Setup(s => s.GetSection("Default")).Returns(_mockDefaultSection.Object);
        _mockConfiguration.Setup(c => c.GetSection("ConnectionStrings")).Returns(_mockConnectionStringsSection.Object);
    }

    [Fact]
    public void AddInfraPersistence_ShouldRegisterDbContext()
    {
        
        _services.AddInfraPersistence(_mockConfiguration.Object);
        var provider = _services.BuildServiceProvider();

       
        var dbContext = provider.GetService<AppDbContext>();
        Assert.NotNull(dbContext);
    }

    [Fact]
    public void AddInfraPersistence_ShouldRegisterGenericRepository()
    {
        
        _services.AddInfraPersistence(_mockConfiguration.Object);
        var provider = _services.BuildServiceProvider();

        
        var customerRepo = provider.GetService<IRepository<Customer>>();
        Assert.NotNull(customerRepo);
        Assert.IsType<Repository<Customer>>(customerRepo);
    }

    [Fact]
    public void AddInfraPersistence_ShouldRegisterRepositoriesAsScoped()
    {
       
        _services.AddInfraPersistence(_mockConfiguration.Object);

        
        var descriptor = _services.FirstOrDefault(d =>
            d.ServiceType == typeof(IRepository<>) &&
            d.ImplementationType == typeof(Repository<>));

        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }
}