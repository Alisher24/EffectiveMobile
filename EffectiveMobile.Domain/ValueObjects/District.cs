using EffectiveMobile.Domain.Shared;

namespace EffectiveMobile.Domain.ValueObjects;

public record District : ValueObject<string>
{
    private const int MaxDistrictLength = 50;

    private District(string value) : base(value)
    {
    }

    public static Result<District> Create(string district)
    {
        if (string.IsNullOrWhiteSpace(district))
            return Error.ValueIsInvalid("District");

        if (district.Length > MaxDistrictLength)
            return Error.ValueIsInvalid("District");

        return new District(district);
    }
}