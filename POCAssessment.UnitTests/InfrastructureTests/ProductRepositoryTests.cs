using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using POCAssessment.Domain;
using POCAssessment.Infrastructure;

namespace POCAssessment.UnitTests.Infrastructure;

public class ProductRepositoryTests
{
    [Fact]
    public async Task WhenGettingProducts_WhenEmptyFilterApplied_ThenAllProductsReturned()
    {
        var url = "https//:correctUrl";
        var products = new List<Product>
            {
                new Product()
                {
                    Title = "Product 1",
                    Price = 50,
                    Sizes = ["S", "M", "L"],
                    Description = "This is a description"
                },
                new Product()
                {
                    Title = "Product 2",
                    Price = 80,
                    Sizes = [ "M", "L", "XL"],
                    Description = "This is a second blue description"
                }

            };
        var ctx = new Mock<IDataContext>();
        ctx.Setup(dc => dc.GetProductsFromApi(url)).ReturnsAsync(products.AsEnumerable<Product>);

        var appSettings = Options.Create<DataSettings>(new DataSettings() { ProductDataLocation = url });

        var repo = new ProductRepository(ctx.Object, appSettings);

        var result = await repo.GetProducts(new ProductFilter(int.MinValue, int.MaxValue, ""));

        result.Should().NotBeEmpty();
        result.Should().NotBeNull();
        result.Count().Should().Be(2);


    }

    [Fact]
    public async Task WhenGettingProducts_WhenFilterApplied_ThenFilteredProductsReturned()
    {
        var url = "https//:correctUrl";
        var products = new List<Product>
            {
                new Product()
                {
                    Title = "Product 1",
                    Price = 50,
                    Sizes = ["S", "M", "L"],
                    Description = "This is a description"
                },
                new Product()
                {
                    Title = "Product 2",
                    Price = 80,
                    Sizes = [ "M", "L", "XL"],
                    Description = "This is a second blue description"
                }

            };
        var ctx = new Mock<IDataContext>();
        ctx.Setup(dc => dc.GetProductsFromApi(url)).ReturnsAsync(products.AsEnumerable<Product>);

        var appSettings = Options.Create<DataSettings>(new DataSettings() { ProductDataLocation = url });

        var repo = new ProductRepository(ctx.Object, appSettings);

        var result = await repo.GetProducts(new ProductFilter(60, 100, ""));

        result.Should().NotBeEmpty();
        result.Should().NotBeNull();
        result.Count().Should().Be(1);
        result.First().Title.Should().Be("Product 2");
    }

    [Fact]
    public async Task WhenGettingProducts_WhenFilterAppliedWithSize_ThenFilteredProductsReturned()
    {
        var url = "https//:correctUrl";
        var products = new List<Product>
            {
                new Product()
                {
                    Title = "Product 1",
                    Price = 50,
                    Sizes = ["S", "M", "L"],
                    Description = "This is a description"
                },
                new Product()
                {
                    Title = "Product 2",
                    Price = 80,
                    Sizes = [ "M", "L", "XL"],
                    Description = "This is a second blue description"
                }

            };
        var ctx = new Mock<IDataContext>();
        ctx.Setup(dc => dc.GetProductsFromApi(url)).ReturnsAsync(products.AsEnumerable<Product>);

        var appSettings = Options.Create<DataSettings>(new DataSettings() { ProductDataLocation = url });

        var repo = new ProductRepository(ctx.Object, appSettings);

        var result = await repo.GetProducts(new ProductFilter(60, 100, "M"));

        result.Should().NotBeEmpty();
        result.Should().NotBeNull();
        result.Count().Should().Be(1);
        result.First().Title.Should().Be("Product 2");
    }

    [Fact]
    public async Task WhenGettingProducts_WhenNoProductsReturned_ThenEmptyListReturned()
    {
        var url = "https//:correctUrl";
        var products = new List<Product>();

        var ctx = new Mock<IDataContext>();
        ctx.Setup(dc => dc.GetProductsFromApi(url)).ReturnsAsync(products.AsEnumerable<Product>);

        var appSettings = Options.Create<DataSettings>(new DataSettings() { ProductDataLocation = url });

        var repo = new ProductRepository(ctx.Object, appSettings);

        var result = await repo.GetProducts(new ProductFilter(60, 100, "M"));

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenGettingProducts_WhenNoProductsReturnedAndFilterEmpty_ThenEmptyListReturned()
    {
        var url = "https//:correctUrl";
        var products = new List<Product>();

        var ctx = new Mock<IDataContext>();
        ctx.Setup(dc => dc.GetProductsFromApi(url)).ReturnsAsync(products.AsEnumerable<Product>);

        var appSettings = Options.Create<DataSettings>(new DataSettings() { ProductDataLocation = url });

        var repo = new ProductRepository(ctx.Object, appSettings);

        var result = await repo.GetProducts(new ProductFilter(int.MinValue, int.MaxValue, ""));

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenGettingProducts_WhenNullReturnedFromApi_ThenEmptyListReturned()
    {
        var url = "https//:correctUrl";
        List<Product>? products = null;
        var ctx = new Mock<IDataContext>();
        ctx.Setup(dc => dc.GetProductsFromApi(url)).ReturnsAsync(products);

        var appSettings = Options.Create<DataSettings>(new DataSettings() { ProductDataLocation = url });

        var repo = new ProductRepository(ctx.Object, appSettings);

        var result = await repo.GetProducts(new ProductFilter(60, 100, "M"));

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}