using BugStore.Application.Services.Dtos.OrderLines.Request;
using FluentValidation;

namespace BugStore.Application.Validators;
public class OrderLineRequestValidator : AbstractValidator<OrderLineRequest>
{
    public OrderLineRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("O ProductId é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("O ProductId não pode ser vazio");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero");
    }
}

