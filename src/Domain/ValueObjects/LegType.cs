namespace Domain.ValueObjects;

public sealed record LegType(NonEmptyString Value)
{
    public NonEmptyString Value { get; init; } = 
        LegTypeList.LegTypes.Contains(Value.ToUpper().ToString()) ? Value.ToUpper()
        : throw new ArgumentNullException($"{Value} is not a valid leg type.", nameof(Value));
}