using EffectiveMobile.Domain.ValueObjects;

namespace EffectiveMobile.Domain;

public class Order(
    Guid id,
    Weight weight,
    District district,
    DeliveryTime deliveryTime)
{
    public Guid Id { get; private set; } = id;

    public Weight Weight { get; private set; } = weight;

    public District District { get; private set; } = district;

    public DeliveryTime DeliveryTime { get; private set; } = deliveryTime;
}