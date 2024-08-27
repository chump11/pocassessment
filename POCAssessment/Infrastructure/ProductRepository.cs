using Microsoft.Extensions.Options;
using POCAssessment.Domain;

namespace POCAssessment.Infrastructure;

public class ProductRepository : IProductRepository
{
    private readonly IDataContext _dataContext;
    private readonly IOptions<DataSettings> _appSettings;

    public ProductRepository(IDataContext dataContext, IOptions<DataSettings> appSettings)
    {
        _dataContext = dataContext;
        _appSettings = appSettings;
    }

    /// <summary>
    /// This method mimics the call to the database to retrieve the list of Products with filtering applied.
    /// I decided to retrieve all the products as an IEnumerable and then apply the filter once 
    /// with the call to ToList() is made to prevent additional lists being created.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> GetProducts(ProductFilter filter)
    {
        var url = _appSettings.Value.ProductDataLocation;
        var products = await _dataContext.GetProductsFromApi(url).ConfigureAwait(false);
        if (products == null)
        {
            return new List<Product>();
        }

        //filter the products
        products = products.Where(p => p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice);

        if (!string.IsNullOrEmpty(filter.Size))
        {
            products = products.Where(p => p.Sizes.Contains(filter.Size));
        }
        return products;
    }

}
