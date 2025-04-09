using EventSourcing.src.Domain.Aggregates;
using EventSourcing.src.Domain.Events;
using EventSourcing.src.Domain.ValueObjects;
using EventSourcingTest.TestHelpers;
using FluentAssertions;

namespace EventSourcingTest.Domain.Aggregates;
public class BankAccountTest
{
    [Fact]
    public void Open_WithValidParameters_CreatesAccountWithInitialDeposit()
    {
        // Arrange
        const string accountHolder = "John Doe";
        var initialDeposit = new PositiveDecimal(1000m);

        // Act
        var account = BankAccount.Open(accountHolder, initialDeposit);

        // Assert
        account.AccountHolder.Should().Be(accountHolder);
        account.Balance.Should().Be(1000m);
        account.Events.Should().ContainSingle()
            .Which.Should().BeOfType<BankAccountOpened>();
        account.IsClosed.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Open_WithInvalidAccountHolder_ThrowsException(string invalidName)
    {
        // Arrange
        var initialDeposit = new PositiveDecimal(1000m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            BankAccount.Open(invalidName, initialDeposit));
    }

    [Fact]
    public void Deposit_ValidAmount_IncreasesBalanceAndRecordsEvent()
    {
        // Arrange
        var account = BankAccountTestBuilder.Create()
            .WithInitialBalance(500m)
            .Build();

        // Act
        account.Deposit(new PositiveDecimal(300), "Salary");

        // Assert
        account.Balance.Should().Be(800m);
        account.Events.Should().HaveCount(2);
        account.Events.Last().Should().BeOfType<FundsDeposited>();
    }

    [Fact]
    public async Task Withdraw_WhenInsufficientFunds_ThrowsException()
    {
        // Arrange
        var account = BankAccountTestBuilder.Create()
            .WithInitialBalance(100m)
            .Build();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            account.Withdraw(new PositiveDecimal(200), "Overdraft"));
    }

    [Fact]
    public void CloseAccount_WithZeroBalance_ClosesAccount()
    {
        // Arrange
        var account = BankAccountTestBuilder.Create()
            .WithInitialBalance(100m)
            .Build();

        account.Withdraw(new PositiveDecimal(100), "Withdraw all");

        // Act
        account.CloseAccount("By request");

        // Assert
        account.IsClosed.Should().BeTrue();
        account.Events.Should().HaveCount(3);
        account.Events.Last().Should().BeOfType<BankAccountClosed>();
    }
}
