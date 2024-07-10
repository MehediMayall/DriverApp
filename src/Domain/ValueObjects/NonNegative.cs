namespace Domain.ValueObjects;

public sealed record NonNegative(int Value)
{
    public int Value {get; init;} = 
        Value > -1 ? Value:
        throw new ArgumentNullException("Value can't be negative", nameof(Value));

    public static implicit operator int(NonNegative Value) => Value.Value;
    public static explicit operator NonNegative(int Value) => new(Value);

}