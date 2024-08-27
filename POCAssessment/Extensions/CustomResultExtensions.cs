using POCAssessment.Common;

namespace POCAssessment.Extensions;

/// <summary>
/// Provides a standard way of returning a ProblemDetails results from a failed Result object
/// </summary>
public static class CustomResultExtensions
{

    public static IResult ToProblemDetailsBadRequest<T>(this Result<T> result) =>
        result.IsSuccess
            ? throw new InvalidOperationException("Cannot convert a successful result to a problem details")
            : Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad request",
                type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                extensions: new Dictionary<string, object?>
                {
                    { "errors", result.Error }
                });
}
