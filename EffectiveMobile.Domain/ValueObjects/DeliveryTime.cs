using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Domain.ValueObjects;

public record DeliveryTime : ValueObject<DateTime>
{
    private DeliveryTime(DateTime value) : base(value)
    {
    }

    public static Result<DeliveryTime> Create(string deliveryTime)
    {
        var parseResult = DateTime.TryParse(deliveryTime, out var deliveryTimeParse);
        if (parseResult == false)
            return Error.ValueIsInvalid("Delivery time");

        if (deliveryTimeParse < DateTime.Now)
            return Error.ValueIsInvalid("Delivery time");

        return new DeliveryTime(deliveryTimeParse);
    }
}