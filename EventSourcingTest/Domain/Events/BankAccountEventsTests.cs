using EventSourcing.src.Domain.Events;
using FluentAssertions;

namespace EventSourcingTest.Domain.Events;
public class BankAccountEventsTests
{
    [Fact]
    public void BankAccountOpened_ShouldHaveCorrectProperties()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        const string holder = "Test User";
        const decimal balance = 1000m;

        // Act
        var @event = new BankAccountOpened(accountId, holder, balance);

        // Assert
        @event.AccountId.Should().Be(accountId);
        @event.AccountHolder.Should().Be(holder);
        @event.InitialBalance.Should().Be(balance);
        @event.Currency.Should().Be("USD");
        @event.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AllEvents_ShouldHaveAccountIdAndTimestamp()
    {
        var accountId = Guid.NewGuid();

        var events = new BankAccountEvent[]
        {
            new FundsDeposited(accountId, 100m, "Deposit"),
            new FundsWithdrawn(accountId, 50m, "Withdrawal"),
            new FundsTransferred(accountId, 30m, Guid.NewGuid(), "Transfer"),
            new BankAccountClosed(accountId, "Reason")
        };

        foreach (var @event in events)
        {
            @event.AccountId.Should().Be(accountId);
            @event.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
