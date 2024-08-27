namespace POCAssessment.Application.Products;

public record ProductResponseDto(string Title, int Price, List<string> Sizes, string Description);
