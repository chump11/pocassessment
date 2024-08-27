using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using POCAssessment.Common;
using POCAssessment.Extensions;

namespace POCAssessment.UnitTests.ExtensionsTests;

public class CustomResultExtensionsTests
{
    [Fact]
    public void ToProblemDetailsBadRequest_WithSuccessfulResult_ThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result<string>.SuccessResult("Success");

        // Act & Assert

        Assert.Throws<InvalidOperationException>(() => result.ToProblemDetailsBadRequest());
        Action act = () => result.ToProblemDetailsBadRequest();

        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Cannot convert a successful result to a problem details");
    }

    [Fact]
    public void ToProblemDetailsBadRequest_WithFailedResult_ReturnsProblemResultWithBadRequestStatusCode()
    {
        // Arrange
        var error = "Invalid input";
        var result = Result<string>.FailedResult(new Error("400", error));

        // Act
        var problemResult = result.ToProblemDetailsBadRequest() as ProblemHttpResult;

        // Assert
        problemResult.Should().NotBeNull();
        problemResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        problemResult?.ProblemDetails.Title.Should().Be("Bad request");
        problemResult?.ProblemDetails.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problemResult?.ProblemDetails.Extensions.Should().ContainKey("errors");
    }
}
