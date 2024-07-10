namespace Domain.ValueObjects;

public record NonEmptyString(string Value)
{
    public string Value {get; init;} = 
        !string.IsNullOrEmpty(Value) ? Value.Trim() 
        : throw new ArgumentException("Value must be non-empty", nameof(Value));

    public static implicit operator string(NonEmptyString value) => value.Value;
    public static explicit operator NonEmptyString(string value) => new(value);

    public NonEmptyString ToUpper() => new(Value.ToUpper());
    public override string ToString() => Value;
}