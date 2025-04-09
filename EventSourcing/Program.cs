using EventSourcing.src.Domain.Aggregates;
using EventSourcing.src.Domain.ValueObjects;

Console.WriteLine("Event Sourcing Case");

// Create new account
var account = BankAccount.Open(
    "John Doe",
    new PositiveDecimal(1000),
    Currency.USD);

// Perform transactions
account.Deposit(500, "Salary deposit");
account.Withdraw(200, "ATM withdrawal");
account.Transfer(300, Guid.NewGuid(), "Transfer to savings");

// Close account
account.Withdraw(account.Balance, "Closing balance withdrawal");
account.CloseAccount("Account holder request");

// Rebuild from events
var rebuiltAccount = BankAccount.FromEvents(account.Events);

#region "Temp"

//// Base event type
//public abstract record Event(Guid StreamId)
//{
//    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
//}

//// Spesific events for bank account domain
//public record AccountOpened(
//    Guid AccountId,
//    string AccountHolder,
//    decimal InitialDeposit,
//    string Currency = "USD") : Event(AccountId);

//public record MoneyDeposited(
//    Guid AccountId,
//    decimal Amount,
//    string Description) : Event(AccountId);

//public record MoneyWithdrawn(
//    Guid AccountId,
//    decimal Amount,
//    string Description) : Event(AccountId);

//public record MoneyTransferred(
//    Guid AccountId,
//    decimal Amount,
//    Guid ToAccountId,
//    string Description) : Event(AccountId);

//public record AccountClosed(
//    Guid AccountId,
//    string Reason) : Event(AccountId);

//// Domain aggregate
//public class BankAccount
//{
//    public Guid Id { get; private set; }
//    public string AccountHolder { get; private set; }
//    public decimal Balance { get; private set; }
//    public string Currency { get; private set; }
//    public bool IsActive { get; private set; }

//    public List<Event> Events { get; } = [];

//    private BankAccount()
//    {

//    }

//    public static BankAccount Open(string accountHolder, decimal initialDeposit, string currency = "USD")
//    {
//        if (string.IsNullOrWhiteSpace(accountHolder))
//        {
//            throw new ArgumentException("Account holder name is required");
//        }

//        if (initialDeposit < 0)
//        {
//            throw new ArgumentException("the initial deposit can't be negative");
//        }

//        var bankAccount = new BankAccount();

//        var @event = new AccountOpened(Guid.NewGuid(), accountHolder, initialDeposit, currency);

//        bankAccount.Apply(@event);

//        return bankAccount;
//    }

//    public void Deposit(decimal amount, string description)
//    {
//        EnsureAccountIsActive();
//        if (amount <= 0) 
//        {
//            throw new ArgumentException("Deposit amount must be positive");
//        }

//        Apply(new MoneyDeposited(Id, amount, description));
//    }

//    public void Withdraw(decimal amount, string description) 
//    {
//        EnsureAccountIsActive();
//        if(amount <= 0)
//        {
//            throw new ArgumentException("Withdraw amount must be positive");
//        }

//        if (Balance - amount < 0) 
//        {
//            throw new InvalidOperationException("Insufficient funds");
//        }

//        Apply(new MoneyWithdrawn(Id, amount, description));
//    }

//    public void TransferTo(Guid toAccountId, decimal amount, string description)
//    {
//        EnsureAccountIsActive();
//        if (amount <= 0)
//        {
//            throw new ArgumentException("Deposit amount must be positive");
//        }

//        if (Balance - amount < 0) 
//        {
//            throw new ArgumentException("Insufficient funds");
//        }

//        Apply(new MoneyTransferred(Id, amount, toAccountId, description));
//    }

//    public void Close(string reason)
//    {
//        EnsureAccountIsActive();
//        if (Balance != 0)
//        {
//            throw new ArgumentException("Cannot close account with non-zero balance");
//        }

//        Apply(new AccountClosed(Id, reason));
//    }

//    private void Apply(Event @event)
//    {
//        switch (@event)
//        {
//            case AccountOpened e:
//                Id = e.AccountId;
//                AccountHolder = e.AccountHolder;
//                Balance = e.InitialDeposit;
//                Currency = e.Currency;
//                IsActive = true;
//                break;
//            case MoneyDeposited e:
//                Balance += e.Amount;
//                break;
//            case MoneyWithdrawn e:
//                Balance -= e.Amount;
//                break;
//            case MoneyTransferred e:
//                Balance -= e.Amount;
//                break;
//            case AccountClosed e:
//                IsActive = false;
//                break;
//        }

//        Events.Add(@event);
//    }


//    public static BankAccount ReplayEvents(IEnumerable<Event> events)
//    {
//        var bankAccount = new BankAccount();
//        foreach (Event @event in events)
//        {
//            bankAccount.Apply(@event);
//        }
//        return bankAccount;
//    }

//    private void EnsureAccountIsActive()
//    {
//        if (!IsActive)
//        {
//            throw new InvalidOperationException("Account is closed");
//        }
//    }
//}

#endregion