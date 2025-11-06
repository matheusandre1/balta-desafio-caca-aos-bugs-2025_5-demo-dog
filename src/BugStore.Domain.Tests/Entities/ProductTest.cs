using AutoFixture;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Tests.Entities;
public class ProductTest
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]

    public void TheProductFieldCannotBeEmpty()
    {
        var product = _fixture.Build<Product>().Create();

        Assert.NotNull(product);
    }

    [Fact]

    public void TheProductFieldIDCannotBeEmpty()
    {
        var product = _fixture.Build<Product>().Create();

        var guid = Guid.Empty;

        Assert.NotEqual(guid, product.Id);
    }

    [Fact]
    public void TheProductFieldTitleCannotBeEmpty()
    {
        var product = _fixture.Build<Product>().Create();


        Assert.NotEqual("", product.Title);
    }

    [Fact]
    public void TheProductFieldDescriptionCannotBeEmpty()
    {
        var product = _fixture.Build<Product>().Create();


        Assert.NotEqual("", product.Description);
    }

}
