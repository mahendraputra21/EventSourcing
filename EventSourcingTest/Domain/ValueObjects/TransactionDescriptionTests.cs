using EventSourcing.src.Domain.ValueObjects;
using FluentAssertions;

namespace EventSourcingTest.Domain.ValueObjects;
public class TransactionDescriptionTests
{
    [Theory]
    [InlineData("Valid description")]
    [InlineData("Another valid one")]
    public void Create_WithValidText_ShouldSucceed(string validText)
    {
        // Act
        var description = new TransactionDescription(validText);

        // Assert
        description.Value.Should().Be(validText);
    }

    [Fact]
    public void ImplicitConversion_ShouldWorkBothWays()
    {
        // Arrange
        const string text = "Test description";

        // Act
        TransactionDescription description = text;
        string convertedBack = description;

        // Assert
        convertedBack.Should().Be(text);
    }
}
