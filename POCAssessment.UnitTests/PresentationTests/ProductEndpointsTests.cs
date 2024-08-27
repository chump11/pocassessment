using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using POCAssessment.Application.Products;
using POCAssessment.Common;
using POCAssessment.Presentation;

namespace POCAssessment.UnitTests.PresentationTests
{
    public class ProductEndpointsTests
    {
        [Fact]
        public async Task ProductEndpoints_WhenResultIsSuccess_ShouldReturnListResponse()
        {
            // Arrange
            var productController = new ProductEndpoints();
            var mediator = new Mock<IMediator>();
            var filter = new ProductFilterQuery(null, null, null, null);
            mediator.Setup(x => x.Send(filter, It.IsAny<CancellationToken>())).ReturnsAsync(new Result<List<ProductResponseDto>>(true, Error.None, []));

            // Act
            var response = await productController.GetFilteredProducts(filter, mediator.Object);

            // Assert
            response.Should().BeOfType<Ok<List<ProductResponseDto>>>();
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task ProductEndpoints_WhenResultIsFailure_ShouldReturnProblemHttpResult()
        {
            // Arrange
            var productController = new ProductEndpoints();
            var mediator = new Mock<IMediator>();
            var filter = new ProductFilterQuery(null, null, null, null);
            mediator.Setup(x => x.Send(filter, It.IsAny<CancellationToken>())).ReturnsAsync(new Result<List<ProductResponseDto>>(false, new Error("400", "Error Found"), []));

            // Act
            var response = await productController.GetFilteredProducts(filter, mediator.Object);

            // Assert
            var castResponse = response as ProblemHttpResult;
            castResponse.Should().NotBeNull();
            castResponse?.ProblemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
            (castResponse?.ProblemDetails.Extensions["errors"] as Error)?.Description.Should().Be("Error Found");
        }
    }
}
