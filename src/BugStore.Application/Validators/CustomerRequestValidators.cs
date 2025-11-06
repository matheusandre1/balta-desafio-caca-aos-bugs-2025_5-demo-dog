namespace BugStore.Application.Validators;
using FluentValidation;
using global::BugStore.Application.Services.Customers.Dto.Request;



public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {        
             RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome é Obrigatório")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Nome é Obrigatório")
            .MaximumLength(50)
            .WithMessage("O Nome Deve Ter No Máximo 50 Caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email é Obrigatório")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Email é Obrigatório")
            .EmailAddress()
            .WithMessage("Email inválido");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Telefone é Obrigatório")
            .Matches(@"^\d{10,11}$")
            .WithMessage("O Telefone deve ter entre 10 e 11 dígitos");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("A Data De Nascimento É Obrigatória")
            .LessThan(DateTime.Now)
            .WithMessage("A Data Tem que ser Válida");
           
    }
    
}
