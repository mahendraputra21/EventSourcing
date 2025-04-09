using EventSourcing.src.Domain.Aggregates;
using EventSourcing.src.Domain.Events;
using EventSourcing.src.Domain.ValueObjects;
using EventSourcingTest.TestHelpers;
using FluentAssertions;

namespace EventSourcingTest.Integration;
public class BankAccountIntegrationTests
{
    [Fact]
    public void Open_WithValidParameters_CreatesAccountWithInitialDeposit()
    {
        // Arrange
        var accountHolder = "John Doe";
        var initialDeposit = new PositiveDecimal(1000m);

        // Act
        var account = BankAccount.Open(accountHolder, initialDeposit);

        // Assert
        Assert.Equal(accountHolder, account.AccountHolder);
        Assert.Equal(1000m, account.Balance);
        Assert.Single(account.Events);
        Assert.IsType<BankAccountOpened>(account.Events.First());
    }

    [Fact]
    public void Deposit_ValidAmount_IncreasesBalance()
    {
        // Arrange
        var account = BankAccount.Open("Test", new PositiveDecimal(500));

        // Act
        account.Deposit(new PositiveDecimal(300), "Test deposit");

        // Assert
        Assert.Equal(800m, account.Balance);
        Assert.Equal(2, account.Events.Count);
    }

    [Fact]
    public async Task Withdraw_WhenInsufficientFunds_ThrowsException()
    {
        // Arrange
        var account = BankAccount.Open("Test", new PositiveDecimal(100));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            account.Withdraw(new PositiveDecimal(200), "Overdraft attempt"));
    }

    [Fact]
    public void ReplayEvents_ShouldReconstructSameAccount()
    {
        // Arrange
        var original = BankAccountTestBuilder.Create()
            .WithInitialBalance(1000m)
            .Build();

        original.Deposit(new PositiveDecimal(500), "Deposit 1");
        original.Withdraw(new PositiveDecimal(200), "Withdrawal 1");
        original.Transfer(new PositiveDecimal(300), Guid.NewGuid(), "Transfer 1");

        // Act
        var reconstructed = BankAccount.FromEvents(original.Events);

        // Assert
        reconstructed.Should().BeEquivalentTo(original, options =>
            options.Excluding(a => a.Events));

        reconstructed.Events.Should().BeEquivalentTo(original.Events);
    }

    [Fact]
    public void ClosedAccount_ShouldNotAcceptNewTransactions()
    {
        // Arrange
        var account = BankAccountTestBuilder.Create()
            .WithInitialBalance(1000m)
            .Build();

        account.Withdraw(new PositiveDecimal(1000), "Empty account");
        account.CloseAccount("By request");

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            account.Deposit(new PositiveDecimal(100), "Attempt after closure"));
    }
}
