using BugStore.Application.Services.Products.Dto.Request;
using FluentValidation;

namespace BugStore.Application.Validators;
public class ProductRequestValidators : AbstractValidator<ProductRequest>
{
    public ProductRequestValidators()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title é Obrigatório")
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Title é Obrigatório");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description é Obrigatório")
            .Must(description => !string.IsNullOrWhiteSpace(description))
            .WithMessage("Description é Obrigatório");

        RuleFor(x=> x.Price)            
            .NotEmpty()
            .WithMessage("Price é obrigatório")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price não pode ser menor que zero");



    }
}
