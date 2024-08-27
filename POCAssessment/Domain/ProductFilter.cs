namespace POCAssessment.Domain;

public class ProductFilter
{
    public ProductFilter(int minPrice, int maxPrice, string? size)
    {
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        Size = size;
    }
    public int MaxPrice { get; private set; }
    public int MinPrice { get; private set; }
    public string? Size { get; private set; }
}
