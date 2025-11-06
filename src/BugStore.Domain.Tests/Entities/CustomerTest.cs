using AutoFixture;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Tests.Entities;
public class CustomerTest
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void TheCustomerFieldCannotBeEmpty() 
    {
        var customer = _fixture
            .Build<Customer>().Create();       

        Assert.NotNull(customer);
    }

    [Fact]

    public void TheCustomerFieldIDCannotBeEmpty()
    {
        var customer = _fixture
            .Build<Customer>().Create();

        Assert.NotEqual(Guid.Empty,customer.Id);
    }

    [Fact]

    public void TheCustomerFieldNameCannotBeEmpty()
    {
        var customer = _fixture
            .Build<Customer>().Create();



        Assert.NotEqual("", customer.Name);
    }

    [Fact]
    public void TheCustomerFieldEmailCannotBeEmpty()
    {
        var customer = _fixture
            .Build<Customer>().Create();

        Assert.NotEqual("", customer.Email);
    }

    [Fact]
    public void TheCustomerFieldBirthDateCannotBeEmpty()
    {
        var customer = _fixture
            .Build<Customer>().Create();

        
        Assert.NotEqual(DateTime.Now, customer.BirthDate);
    }
}
