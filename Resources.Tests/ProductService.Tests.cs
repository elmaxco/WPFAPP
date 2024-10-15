using Moq;
using Xunit;
using Resources.Services;
using Resources.Models;
using Resources.Enums;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class ProductServiceTests
{
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _fileServiceMock = new Mock<IFileService>();
        _productService = new ProductService(_fileServiceMock.Object);
    }

    [Fact]
    public void AddToList_ShouldAddProduct_WhenProductIsValid()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Price = 100, Category = "Electronics" };
        _fileServiceMock.Setup(f => f.GetFromFile()).Returns(string.Empty); // Tom fil
        _fileServiceMock.Setup(f => f.SaveToFile(It.IsAny<string>())).Returns(true);

        // Act
        var result = _productService.AddToList(product);

        // Assert
        Assert.Equal(ResultStatus.Success, result);
        _fileServiceMock.Verify(f => f.SaveToFile(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void AddToList_ShouldReturnExists_WhenProductAlreadyExists()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Price = 100, Category = "Electronics" };
        var productList = new List<Product> { product };
        _fileServiceMock.Setup(f => f.GetFromFile()).Returns(JsonConvert.SerializeObject(productList));

        // Act
        var result = _productService.AddToList(product);

        // Assert
        Assert.Equal(ResultStatus.Exists, result);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnProductList_FromFile()
    {
        // Arrange
        var productList = new List<Product>
        {
            new Product { Name = "Test Product 1", Price = 100, Category = "Electronics" },
            new Product { Name = "Test Product 2", Price = 200, Category = "Clothing" }
        };
        _fileServiceMock.Setup(f => f.GetFromFile()).Returns(JsonConvert.SerializeObject(productList));

        // Act
        var result = _productService.GetAllProducts();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Test Product 1", result.First().Name);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Price = 100, Category = "Electronics", ID = "1" };
        var productList = new List<Product> { product };
        _fileServiceMock.Setup(f => f.GetFromFile()).Returns(JsonConvert.SerializeObject(productList));
        _fileServiceMock.Setup(f => f.SaveToFile(It.IsAny<string>())).Returns(true);

        // Act
        var result = _productService.DeleteProduct(product);

        // Assert
        Assert.Equal(ResultStatus.Success, result);
        _fileServiceMock.Verify(f => f.SaveToFile(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productList = new List<Product>();
        _fileServiceMock.Setup(f => f.GetFromFile()).Returns(JsonConvert.SerializeObject(productList));

        var product = new Product { Name = "Non-existing Product", ID = "999" };

        // Act
        var result = _productService.DeleteProduct(product);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result);
    }
}
