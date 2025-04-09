using EventSourcing.src.Domain.Events;
using EventSourcing.src.Domain.ValueObjects;

namespace EventSourcing.src.Domain.Aggregates;
public class BankAccount
{
    public Guid Id { get; private set; }
    public string AccountHolder { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public Currency Currency { get; private set; } = Currency.USD;
    public bool IsClosed { get; private set; }
    public IReadOnlyList<BankAccountEvent> Events => _events.AsReadOnly();

    private readonly List<BankAccountEvent> _events = new();

    private BankAccount() { }

    public static BankAccount Open(
        string accountHolder,
        PositiveDecimal initialDeposit,
        Currency? currency = null)
    {
        if (string.IsNullOrWhiteSpace(accountHolder))
            throw new ArgumentException("Account holder name cannot be empty", nameof(accountHolder));

        var account = new BankAccount();
        var openedEvent = new BankAccountOpened(
            Guid.NewGuid(),
            accountHolder.Trim(),
            initialDeposit,
            currency ?? Currency.USD);

        account.ApplyEvent(openedEvent);
        return account;
    }

    public async Task Deposit(PositiveDecimal amount, TransactionDescription description)
    {
        EnsureAccountIsActive();
        var depositedEvent = new FundsDeposited(Id, amount, description);
        ApplyEvent(depositedEvent);
    }

    public async Task Withdraw(PositiveDecimal amount, TransactionDescription description)
    {
        EnsureAccountIsActive();

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds for withdrawal");

        var withdrawnEvent = new FundsWithdrawn(Id, amount, description);
        ApplyEvent(withdrawnEvent);
    }

    public void Transfer(
        PositiveDecimal amount,
        Guid destinationAccountId,
        TransactionDescription description)
    {
        EnsureAccountIsActive();

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds for transfer");

        var transferredEvent = new FundsTransferred(Id, amount, destinationAccountId, description);
        ApplyEvent(transferredEvent);
    }

    public void CloseAccount(string reason)
    {
        EnsureAccountIsActive();

        if (Balance != 0)
            throw new InvalidOperationException("Cannot close account with non-zero balance");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Closure reason must be provided", nameof(reason));

        var closedEvent = new BankAccountClosed(Id, reason.Trim());
        ApplyEvent(closedEvent);
    }

    public static BankAccount FromEvents(IEnumerable<BankAccountEvent> events)
    {
        var account = new BankAccount();
        foreach (var @event in events)
        {
            account.ApplyEvent(@event);
        }
        return account;
    }

    private void ApplyEvent(BankAccountEvent @event)
    {
        switch (@event)
        {
            case BankAccountOpened e:
                Id = e.AccountId;
                AccountHolder = e.AccountHolder;
                Balance = e.InitialBalance;
                Currency = e.Currency;
                break;

            case FundsDeposited e:
                Balance += e.Amount;
                break;

            case FundsWithdrawn e:
                Balance -= e.Amount;
                break;

            case FundsTransferred e:
                Balance -= e.Amount;
                break;

            case BankAccountClosed:
                IsClosed = true;
                break;
        }

        _events.Add(@event);
    }

    private void EnsureAccountIsActive()
    {
        if (IsClosed)
            throw new InvalidOperationException("Account is closed");
    }
}