using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using POCAssessment.Application.Products;
using POCAssessment.Common;
using POCAssessment.Domain;

namespace POCAssessment.UnitTests.ApplicationTests;

public class ProductFilterQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IValidator<ProductFilterQuery>> _validatorMock;
    private readonly ProductFilterQueryHandler _handler;

    public ProductFilterQueryHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _validatorMock = new Mock<IValidator<ProductFilterQuery>>();
        _handler = new ProductFilterQueryHandler(_productRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsSuccessResult()
    {
        //Arrange
        var request = new ProductFilterQuery(10, 100, "M", "blue");
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None)).ReturnsAsync(validationResult);

        var products = new List<Product>
        {
            new Product()
            {
                Title = "Product 1",
                Price = 50,
                Sizes = ["S", "M", "L"],
                Description = "This is a description"
            },
            new Product()
            {
                Title = "Product 2",
                Price = 80,
                Sizes = [ "M", "L", "XL"],
                Description = "This is a second blue description"
            }

        };
        _productRepositoryMock.Setup(r => r.GetProducts(It.IsAny<ProductFilter>())).ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<List<ProductResponseDto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.Data?[0].Title.Should().Be("Product 1");
        result.Data?[0].Price.Should().Be(50);
        result.Data?[0].Sizes.Should().BeEquivalentTo(new List<string> { "S", "M", "L" });
        result.Data?[0].Description.Should().Be("This is a description");
        result.Data?[1].Title.Should().Be("Product 2");
        result.Data?[1].Price.Should().Be(80);
        result.Data?[1].Sizes.Should().BeEquivalentTo(new List<string> { "M", "L", "XL" });
        result.Data?[1].Description.Should().Be("This is a second <em>blue</em> description");
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ReturnsFailedResult()
    {
        // Arrange
        var request = new ProductFilterQuery(null, null, null, null);
        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("MinPrice", "MinPrice is required."));
        _validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None)).ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<List<ProductResponseDto>>>();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Invalid");
        result.Error.Description.Should().Contain("MinPrice is required.");
    }

    [Fact]
    public async Task Handle_WithHttpRequestException_ReturnsFailedResult()
    {
        var request = new ProductFilterQuery(10, 100, "M", "blue");
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None)).ReturnsAsync(validationResult);

        _productRepositoryMock.Setup(r => r.GetProducts(It.IsAny<ProductFilter>())).ThrowsAsync(new HttpRequestException("Internal Server Error"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<List<ProductResponseDto>>>();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Error");
        result.Error.Description.Should().Contain("Internal Server Error");
    }
}
