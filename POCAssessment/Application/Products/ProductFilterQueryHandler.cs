using FluentValidation;
using MediatR;
using POCAssessment.Common;
using POCAssessment.Domain;

namespace POCAssessment.Application.Products;

public class ProductFilterQueryHandler : IRequestHandler<ProductFilterQuery, Result<List<ProductResponseDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<ProductFilterQuery> _validator;

    public ProductFilterQueryHandler(IProductRepository productRepository, IValidator<ProductFilterQuery> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }
    public async Task<Result<List<ProductResponseDto>>> Handle(ProductFilterQuery request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
        if (!result.IsValid)
        {
            return Result<List<ProductResponseDto>>.FailedResult(new Error("Invalid", string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage).ToArray())));
        }

        var productFilter = new ProductFilter(request.MinPrice ?? int.MinValue, request.MaxPrice ?? int.MaxValue, request.Size);
        try
        {
            var products = await _productRepository.GetProducts(productFilter).ConfigureAwait(false);
            var response = new List<ProductResponseDto>();
            foreach (var product in products)
            {
                var dto = new ProductResponseDto(
                    product.Title,
                    product.Price,
                    product.Sizes.ToList(),
                    product.EnrichedDescription(request.Highlight ?? "")
                    );
                response.Add(dto);
            }
            return Result<List<ProductResponseDto>>.SuccessResult(response);
        }
        catch (HttpRequestException ex)
        {
            return Result<List<ProductResponseDto>>.FailedResult(new Error("Error", ex.Message));
        }

    }
}
