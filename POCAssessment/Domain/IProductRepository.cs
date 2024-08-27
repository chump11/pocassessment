
namespace POCAssessment.Domain;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts(ProductFilter filter);
}
