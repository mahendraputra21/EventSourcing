namespace EventSourcing.src.Domain.ValueObjects;
public record PositiveDecimal
{
    public decimal Value { get; }

    public PositiveDecimal(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Value must be positive");
        Value = value;
    }

    public static implicit operator decimal(PositiveDecimal d) => d.Value;
    public static implicit operator PositiveDecimal(decimal d) => new(d);
}
