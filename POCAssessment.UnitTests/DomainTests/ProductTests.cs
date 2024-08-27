using FluentAssertions;
using POCAssessment.Domain;

namespace POCAssessment.UnitTests.DomainTests;

public class ProductTests
{
    [Fact]
    public void Product_WhenPassedAHighlight_ShouldSetHtmlInDescription()
    {
        // Arrange
        var product = new Product()
        {
            Title = "Product 1",
            Price = 50,
            Sizes = ["S", "M", "L"],
            Description = "This is a Red description"
        };
        var highlight = "Red";

        // Act
        var response = product.EnrichedDescription(highlight);

        // Assert
        response.Should().Be("This is a <em>Red</em> description");
    }

    [Fact]
    public void Product_WhenNotPassedAHighlight_ShouldNotSetSetDescription()
    {
        // Arrange
        var product = new Product()
        {
            Title = "Product 1",
            Price = 50,
            Sizes = ["S", "M", "L"],
            Description = "This is a Red description"
        };
        var highlight = "";

        // Act
        var response = product.EnrichedDescription(highlight);

        // Assert
        response.Should().Be("This is a Red description");
    }

    [Fact]
    public void Product_WhenNotPassedAMatchingHighlight_ShouldNotSetSetDescription()
    {
        // Arrange
        var product = new Product()
        {
            Title = "Product 1",
            Price = 50,
            Sizes = ["S", "M", "L"],
            Description = "This is a Red description"
        };
        var highlight = "Blue";

        // Act
        var response = product.EnrichedDescription(highlight);

        // Assert
        response.Should().Be("This is a Red description");
    }

    [Fact]
    public void Product_WhenNoDescriptionPassed_ShouldNotSetSetDescription()
    {
        // Arrange
        var product = new Product()
        {
            Title = "Product 1",
            Price = 50,
            Sizes = ["S", "M", "L"],
            Description = ""
        };
        var highlight = "Blue";

        // Act
        var response = product.EnrichedDescription(highlight);

        // Assert
        response.Should().Be("");
    }
}
