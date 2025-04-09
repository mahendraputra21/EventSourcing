# Event Sourcing Bank Account Implementation Explained
This code demonstrates a complete event-sourced bank account implementation in C#. Let me break it down:

## Core Concepts Implemented
1. Event Sourcing Pattern: Instead of just storing the current balance, we store all transactions as a sequence of events
2. Immutable Events: All state changes are represented as immutable records
3. Event Replay: Ability to reconstruct state by replaying events

## Code Structure
1. Event Definitions

```csharp
public abstract record Event(Guid StreamId)
public record AccountOpened(...) : Event(AccountId);
public record MoneyDeposited(...) : Event(AccountId);
// etc...
```
- Base Event type with common properties (StreamId, Timestamp)
- Specific event types for each action in the bank account domain

2. Bank Account Aggregate

```csharp
public class BankAccount
{
    // Current state properties
    public Guid Id { get; private set; }
    public string AccountHolder { get; private set; }
    public decimal Balance { get; private set; }
    // etc...

    // Event log
    public List<Event> Events { get; } = [];
}
```
- Maintains current state (balance, status)
- Keeps track of all events that occurred

3. Key Methods
Open() - Factory Method

```csharp
public static BankAccount Open(string accountHolder, decimal initialDeposit, string currency = "USD")
{
    // Validation
    var bankAccount = new BankAccount();
    var @event = new AccountOpened(Guid.NewGuid(), accountHolder, initialDeposit, currency);
    bankAccount.Apply(@event);
    return bankAccount;
}
```
- Creates new account by generating an AccountOpened event
- Applies the event to initialize state

Command Methods (Deposit, Withdraw, etc.)

```csharp
public void Deposit(decimal amount, string description)
{
    EnsureAccountIsActive();
    if (amount <= 0) throw /*...*/;
    Apply(new MoneyDeposited(Id, amount, description));
}
```

- Perform business validation
- Generate appropriate event
- Apply the event to modify state

Apply() - State Mutator

```csharp
private void Apply(Event @event)
{
    switch (@event)
    {
        case AccountOpened e:
            Id = e.AccountId;
            AccountHolder = e.AccountHolder;
            Balance = e.InitialDeposit;
            // etc...
            break;
        case MoneyDeposited e:
            Balance += e.Amount;
            break;
        // Other cases...
    }
    Events.Add(@event);
}
```
- Modifies current state based on event type
- Appends event to the event log

ReplayEvents() - Reconstruction

```csharp
public static BankAccount ReplayEvents(IEnumerable<Event> events)
{
    var bankAccount = new BankAccount();
    foreach (Event @event in events)
    {
        bankAccount.Apply(@event);
    }
    return bankAccount;
}
```
- Reconstructs state from event history
- Useful for rebuilding state after restart

## Demo Flow
1. Creates account with initial deposit
2. Performs series of transactions (deposit, withdraw, transfer)
3. Closes account
4. Shows final balance and event history
5. Demonstrates replaying events to reconstruct state
6. Shows error when trying to deposit to closed account

## Key Benefits Illustrated
1. Complete Audit Trail: All changes are recorded as immutable events
2. Temporal Queries: Can see account state at any point in history
3. Event Replay: Can rebuild state from events
4. Business Rules Enforcement: Validations before each operation

This implementation shows the core principles of event sourcing in a banking context, though a production system would typically persist events to a database and might include additional features like snapshots for performance optimization.