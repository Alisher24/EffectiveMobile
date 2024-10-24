using EffectiveMobile.Domain.ValueObjects;
using FluentValidation;

namespace EffectiveMobile.Application.AddOrder;

public class AddOrderValidator : AbstractValidator<AddOrderRequest>
{
    public AddOrderValidator()
    {
        RuleFor(a => a.Weight).MustBeValueObject(Weight.Create);
        RuleFor(a => a.District).MustBeValueObject(District.Create);
        RuleFor(a => a.DeliveryTime).MustBeValueObject(DeliveryTime.Create);
    }
}