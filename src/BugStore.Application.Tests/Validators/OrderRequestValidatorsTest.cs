using AutoFixture;
using BugStore.Application.Services.Dtos.OrderLines.Request;
using BugStore.Application.Services.Orders.Dto.Request;
using BugStore.Application.Validators;
using FluentAssertions;

namespace BugStore.Application.Tests.Validators;

public class OrderRequestValidatorsTest
{
    private readonly Fixture _fixture = new();
    private readonly OrderRequestValidators _validator; 

    public OrderRequestValidatorsTest()
    {
        _validator = new OrderRequestValidators();        
    }

    [Fact]
        
    public void GivenCustomerIdEmpty_WhenValidating_ThenShouldHaveError()
    {
        var validLine = _fixture.Create<OrderLineRequest>();

        var request = _fixture.Build<OrderRequest>()
            .With(x => x.CustomerId, Guid.Empty) 
            .With(x => x.Lines, new List<OrderLineRequest> { validLine })
            .Create();

        var result = _validator.Validate(request);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(e => e.PropertyName == "CustomerId");
    }


    [Fact]
    public void GivenNoLines_WhenValidating_ThenShouldHaveError()
    {
        var request = _fixture.Build<OrderRequest>()
            .With(x => x.CustomerId, Guid.NewGuid())
            .With(x => x.Lines, new List<OrderLineRequest>())
            .Create();

        var result = _validator.Validate(request);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(e => e.PropertyName == "Lines");
    }

    [Fact]
    public void GivenNullLines_WhenValidating_ThenShouldHaveError()
    {
        var request = _fixture.Build<OrderRequest>()
            .With(x => x.CustomerId, Guid.NewGuid())
            .Without(x => x.Lines)
            .Create();

        var result = _validator.Validate(request);

        result.IsValid
            .Should()
            .BeTrue();

        result.Errors
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void GivenLineWithEmptyProductId_WhenValidating_ThenShouldHaveError()
    {
        var invalidLine = _fixture.Build<OrderLineRequest>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        var request = _fixture.Build<OrderRequest>()
            .With(x => x.CustomerId, Guid.NewGuid())
            .With(x => x.Lines, new List<OrderLineRequest> { invalidLine })
            .Create();

        var result = _validator.Validate(request);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(e => e.PropertyName == "Lines[0].ProductId");
    }

    [Fact]
    public void GivenLineWithZeroQuantity_WhenValidating_ThenShouldHaveError()
    {
        var invalidLine = _fixture.Build<OrderLineRequest>()
            .With(x => x.Quantity, 0)
            .Create();

        var request = _fixture.Build<OrderRequest>()
            .With(x => x.CustomerId, Guid.NewGuid())
            .With(x => x.Lines, new List<OrderLineRequest> { invalidLine })
            .Create();

        var result = _validator.Validate(request);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(e => e.PropertyName == "Lines[0].Quantity");
    }
}
