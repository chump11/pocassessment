using FluentAssertions;
using POCAssessment.Application.Products;

namespace POCAssessment.UnitTests.ApplicationTests;

public class ProductFilterQueryValidationTests
{
    [Fact]
    public void ProductFilterQuery_WithValidValues_ShouldNotReturnValidationError()
    {
        // Arrange
        var productFilterQuery = new ProductFilterQuery(0, 100, "M", "highlight");

        // Act
        var validator = new ProductFilterQueryValidation();
        var result = validator.Validate(productFilterQuery);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ProductFilterQuery_WithInvalidMinPrice_ShouldReturnValidationError()
    {
        // Arrange
        var productFilterQuery = new ProductFilterQuery(-1, 100, "M", "highlight");

        // Act
        var validator = new ProductFilterQueryValidation();
        var result = validator.Validate(productFilterQuery);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Min Price is to higher than zero");
    }
}
