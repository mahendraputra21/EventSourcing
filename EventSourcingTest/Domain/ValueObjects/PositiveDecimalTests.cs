using EventSourcing.src.Domain.ValueObjects;
using FluentAssertions;

namespace EventSourcingTest.Domain.ValueObjects;
public class PositiveDecimalTests
{
    [Theory]
    [InlineData(0.01)]
    [InlineData(1)]
    [InlineData(1000000)]
    public void Create_WithPositiveValue_ShouldSucceed(decimal validValue)
    {
        // Act
        var amount = new PositiveDecimal(validValue);

        // Assert
        amount.Value.Should().Be(validValue);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-1000000)]
    public void Create_WithNegativeValue_ShouldThrow(decimal invalidValue)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PositiveDecimal(invalidValue));
    }

    [Fact]
    public void ImplicitConversion_ShouldWorkBothWays()
    {
        // Arrange
        const decimal value = 100m;

        // Act
        PositiveDecimal amount = value;
        decimal convertedBack = amount;

        // Assert
        convertedBack.Should().Be(value);
    }
}
