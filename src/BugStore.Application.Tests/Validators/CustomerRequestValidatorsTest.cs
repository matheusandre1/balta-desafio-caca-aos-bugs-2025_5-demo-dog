using AutoFixture;
using BugStore.Application.Services.Customers.Dto.Request;
using BugStore.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BugStore.Application.Tests.Validators;
public class CustomerRequestValidatorsTest

{
    private readonly Fixture _fixture = new();
    private readonly CustomerRequestValidator _validator;

    public CustomerRequestValidatorsTest() => _validator = new CustomerRequestValidator();


    [Fact]    
    public void GivenANullNameField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {
        
        var request = _fixture
            .Build<CustomerRequest>()
            .With(x => x.Name, (string?)null)
            .Create();

        
        var result = _validator.TestValidate(request);

       
        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Name)
          .WithErrorMessage("Nome é Obrigatório");
    }

    [Fact]
    public void GivenANullNameOrWhiteSpaceField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x => x.Name, "  ")
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Name)
          .WithErrorMessage("Nome é Obrigatório");
    }

    [Fact]
    public void GivenAGreaterThan50CharactersField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x => x.Name, "Alexandra Beatriz Montenegro da Silva e Albuquerque")
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Name)
          .WithErrorMessage("O Nome Deve Ter No Máximo 50 Caracteres");
    }

    [Fact]
    public void GivenANullEmailField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x => x.Email, (string?)null)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Email)
          .WithErrorMessage("Email é Obrigatório");
    }

    [Fact]
    public void GivenAEmailOrWhiteSpaceField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x => x.Email, " " )
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Email)
          .WithErrorMessage("Email é Obrigatório");
    }

    [Fact]
    public void GivenAInvalidEmailIField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x => x.Email, "jose.com.br")
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.Email)
          .WithErrorMessage("Email inválido");
    }

    [Fact]
    public void GivenAInvalidBithDateIField_WhenMakingARequestFromTheClient_ThenReturnAnError()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x=>x.BirthDate, DateTime.Now)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
          .WithErrorMessage("A Data Tem que ser Válida");
    }

    [Fact]
    public void GivenAIFieldInvalid_WhenMakingARequestFromTheClient_ThenReturnAnErrors()
    {

        var request = _fixture
            .Build<CustomerRequest>()
            .With(x=> x.BirthDate, DateTime.Now)
            .Create();


        var result = _validator.TestValidate(request);


        result.Errors
            .Should()
            .NotBeNull();

        
        result.ShouldHaveValidationErrorFor(x => x.Phone)
          .WithErrorMessage("O Telefone deve ter entre 10 e 11 dígitos");

        result.ShouldHaveValidationErrorFor(x => x.Email).
            WithErrorMessage("Email inválido");

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
          .WithErrorMessage("A Data Tem que ser Válida");
    }



}
