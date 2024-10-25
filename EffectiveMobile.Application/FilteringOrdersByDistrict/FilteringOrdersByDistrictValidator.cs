using EffectiveMobile.Domain.Shared;
using EffectiveMobile.Domain.ValueObjects;
using FluentValidation;

namespace EffectiveMobile.Application.FilteringOrdersByDistrict;

public class FilteringOrdersByDistrictValidator : AbstractValidator<FilteringOrdersByDistrictRequest>
{
    public FilteringOrdersByDistrictValidator()
    {
        RuleFor(f => f.District).MustBeValueObject(District.Create);
        RuleFor(f => f.FirstDeliveryTime)
            .Must(f => DateTime.TryParse(f, out _))
            .WithError(Error.ValueIsInvalid("FirstDeliveryTime"));
    }
}