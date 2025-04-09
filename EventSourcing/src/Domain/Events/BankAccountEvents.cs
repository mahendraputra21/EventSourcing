namespace EventSourcing.src.Domain.Events;
public abstract record BankAccountEvent(Guid AccountId, DateTime Timestamp)
{
    protected BankAccountEvent(Guid accountId) : this(accountId, DateTime.UtcNow) { }
}

public record BankAccountOpened(
    Guid AccountId,
    string AccountHolder,
    decimal InitialBalance,
    string Currency = "USD") : BankAccountEvent(AccountId);

public record FundsDeposited(
    Guid AccountId,
    decimal Amount,
    string TransactionDescription) : BankAccountEvent(AccountId);

public record FundsWithdrawn(
    Guid AccountId,
    decimal Amount,
    string TransactionDescription) : BankAccountEvent(AccountId);

public record FundsTransferred(
    Guid AccountId,
    decimal Amount,
    Guid DestinationAccountId,
    string TransactionDescription) : BankAccountEvent(AccountId);

public record BankAccountClosed(
    Guid AccountId,
    string ClosureReason) : BankAccountEvent(AccountId);