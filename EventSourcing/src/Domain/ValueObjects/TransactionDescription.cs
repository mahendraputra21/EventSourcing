namespace EventSourcing.src.Domain.ValueObjects;
public record TransactionDescription(string Value)
{
    public static implicit operator string(TransactionDescription d) => d.Value;
    public static implicit operator TransactionDescription(string s) => new(s);
}
