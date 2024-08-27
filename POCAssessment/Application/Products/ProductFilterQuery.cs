using MediatR;
using POCAssessment.Common;
using System.ComponentModel.DataAnnotations;

namespace POCAssessment.Application.Products;
public record ProductFilterQuery(
    [Range(0, int.MaxValue)]int? MinPrice,
    [Range(0, int.MaxValue)]int? MaxPrice, 
    string? Size, 
    string? Highlight) : IRequest<Result<List<ProductResponseDto>>>;




