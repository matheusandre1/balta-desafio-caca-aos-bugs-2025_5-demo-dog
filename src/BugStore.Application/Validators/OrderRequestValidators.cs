using BugStore.Application.Services.Orders.Dto.Request;
using FluentValidation;

namespace BugStore.Application.Validators;
public  class OrderRequestValidators : AbstractValidator<OrderRequest>
{
    public OrderRequestValidators()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("O Id do Customer é Obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("Customer Não Pode ser Vazio");

        RuleFor(x => x.Lines)
            .NotEmpty()
            .WithMessage("O pedido Deve Conter Pelo Menos Um Item")
            .Must(lines => lines != null && lines.Count > 0)
            .WithMessage("O Pedido Deve Conter Pelo Menos Um Item");

        RuleForEach(x=> x.Lines).SetValidator(new OrderLineRequestValidator());


    }
}
