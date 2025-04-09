using EventSourcing.src.Domain.Aggregates;
using EventSourcing.src.Domain.ValueObjects;

namespace EventSourcingTest.TestHelpers;
public class BankAccountTestBuilder
{
    private string _accountHolder = "Test Account";
    private PositiveDecimal _initialBalance = new(1000m);
    private Currency _currency = Currency.USD;

    public static BankAccountTestBuilder Create() => new();

    public BankAccountTestBuilder WithAccountHolder(string holder)
    {
        _accountHolder = holder;
        return this;
    }

    public BankAccountTestBuilder WithInitialBalance(decimal balance)
    {
        _initialBalance = new PositiveDecimal(balance);
        return this;
    }

    public BankAccountTestBuilder WithCurrency(Currency currency)
    {
        _currency = currency;
        return this;
    }

    public BankAccount Build()
    {
        return BankAccount.Open(_accountHolder, _initialBalance, _currency);
    }
}
