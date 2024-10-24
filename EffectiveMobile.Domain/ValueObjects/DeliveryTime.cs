using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Domain.ValueObjects;

public record DeliveryTime : ValueObject<DateTime>
{
    private DeliveryTime(DateTime value) : base(value)
    {
    }

    public static Result<DeliveryTime> Create(DateTime deliveryTime)
    {
        if (deliveryTime < DateTime.Now)
            return Error.ValueIsInvalid("Delivery time");

        return new DeliveryTime(deliveryTime);
    }
}