using AutoFixture;
using BugStore.Application.Services.Products.Dto.Request;
using BugStore.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BugStore.Application.Tests.Validators;
public class ProductRequestValidatorsTest
{
    private readonly Fixture _fixture = new();
    private readonly ProductRequestValidators _validator;

    public ProductRequestValidatorsTest() => _validator = new ProductRequestValidators();


    [Fact]
    public void GivenANullTitleField_WhenMakingARequestFromTheProduct_ThenReturnAnError()
    {

        var request = _fixture
            .Build<ProductRequest>()
            .With(x => x.Title, (string?)null)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Title)
          .WithErrorMessage("Title é Obrigatório");
    }

    [Fact]
    public void GivenANullTitleWhiteOrSpaceField_WhenMakingARequestFromTheProduct_ThenReturnAnError()
    {

        var request = _fixture
            .Build<ProductRequest>()
            .With(x => x.Title, "  ")
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result
          .ShouldHaveValidationErrorFor(x => x.Title)
          .WithErrorMessage("Title é Obrigatório");
    }

    [Fact]
    public void GivenANullDescriptionField_WhenMakingARequestFromTheProduct_ThenReturnAnError()
    {

        var request = _fixture
            .Build<ProductRequest>()
            .With(x => x.Description, (string?) null)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result
          .ShouldHaveValidationErrorFor(x => x.Description)
          .WithErrorMessage("Description é Obrigatório");
    }

    [Fact]
    public void GivenANullWhiteOrSpaceDescriptionField_WhenMakingARequestFromTheProduct_ThenReturnAnError()
    {

        var request = _fixture
            .Build<ProductRequest>()
            .With(x => x.Description, "  ")
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result
          .ShouldHaveValidationErrorFor(x => x.Description)
          .WithErrorMessage("Description é Obrigatório");
    }


    [Fact]
    public void GivenAPriceInvalidSpaceField_WhenMakingARequestFromTheProduct_ThenReturnAnError()
    {

        var request = _fixture
            .Build<ProductRequest>()
            .With(x => x.Price, 0)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result
          .ShouldHaveValidationErrorFor(x => x.Price)
          .WithErrorMessage("Price é obrigatório");
    }

    [Fact]
    public void GivenAPrice0orLessSpaceField_WhenMakingARequestFromTheProduct_ThenReturnAnError()
    {

        var request = _fixture
            .Build<ProductRequest>()
            .With(x => x.Price, -1)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result
          .ShouldHaveValidationErrorFor(x => x.Price)
          .WithErrorMessage("Price não pode ser menor que zero");
    }
}
