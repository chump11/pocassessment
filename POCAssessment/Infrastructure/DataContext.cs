using POCAssessment.Domain;
using System.Diagnostics.CodeAnalysis;

namespace POCAssessment.Infrastructure;

/// <summary>
/// This interface and class take the role of a dataContext to retrieve the products from the API.
/// Used solely by the repository method.
/// </summary>
public interface IDataContext
{
    Task<IEnumerable<Product>?> GetProductsFromApi(string url);
}
[ExcludeFromCodeCoverage]

internal class DataContext(IHttpClientFactory httpClientFactory) : IDataContext
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<IEnumerable<Product>?> GetProductsFromApi(string url)
    {
        var client = _httpClientFactory.CreateClient("product");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var response = await client.GetFromJsonAsync<IEnumerable<Product>>(url);
        return response;
    }

}
