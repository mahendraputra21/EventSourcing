namespace EventSourcing.src.Domain.ValueObjects;
public record Currency(string Code)
{
    public static readonly Currency USD = new("USD");
    public static implicit operator string(Currency c) => c.Code;
    public static implicit operator Currency(string s) => new(s);
}
