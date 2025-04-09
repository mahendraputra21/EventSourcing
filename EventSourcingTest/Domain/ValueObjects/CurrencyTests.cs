using EventSourcing.src.Domain.ValueObjects;
using FluentAssertions;

namespace EventSourcingTest.Domain.ValueObjects;
public class CurrencyTests
{
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    public void Create_WithValidCode_ShouldSucceed(string validCode)
    {
        // Act
        var currency = new Currency(validCode);

        // Assert
        currency.Code.Should().Be(validCode);
    }

    [Fact]
    public void PredefinedCurrencies_ShouldWork()
    {
        // Act & Assert
        Currency.USD.Code.Should().Be("USD");
    }
}
