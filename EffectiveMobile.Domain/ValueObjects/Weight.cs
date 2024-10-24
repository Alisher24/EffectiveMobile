using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Domain.ValueObjects;

public record Weight : ValueObject<float>
{
    private const float MinWeight = 0.01f;

    private Weight(float value) : base(value)
    {
    }

    public static Result<Weight> Create(float weight)
    {
        if (weight <= MinWeight)
            return Error.ValueIsInvalid("Weight");

        return new Weight(weight);
    }
}