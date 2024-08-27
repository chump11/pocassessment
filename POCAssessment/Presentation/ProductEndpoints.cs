using Carter;
using MediatR;
using POCAssessment.Application.Products;
using POCAssessment.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace POCAssessment.Presentation;

public class ProductEndpoints : CarterModule
{
    public ProductEndpoints() : base($"api/products")
    {

    }
    [ExcludeFromCodeCoverage]
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/filter", GetFilteredProducts)
          .WithName("Product Filter")
          .WithOpenApi()
          .WithTags("Filter");
    }

    public async Task<IResult> GetFilteredProducts([AsParameters] ProductFilterQuery filters, ISender sender)
    {
        var result = await sender.Send(filters);
        return result.IsSuccess
         ? TypedResults.Ok(result.Data)
         : result.ToProblemDetailsBadRequest();
    }
}
