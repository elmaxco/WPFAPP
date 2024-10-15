using Moq;
using Newtonsoft.Json;
using Resources.Enums;
using Resources.Models;
using Resources.Services;
using System.Collections.Generic;
using Xunit;

namespace Resources.Tests;
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
        var product = new Product { ID = "1", Name = "TestProduct" };
        _fileServiceMock.Setup(fs => fs.GetFromFile()).Returns("[]");
        _fileServiceMock.Setup(fs => fs.SaveToFile(It.IsAny<string>())).Returns(true);

        // Act
        var result = _productService.AddToList(product);

        // Assert
        Assert.Equal(ResultStatus.Success, result);
        _fileServiceMock.Verify(fs => fs.SaveToFile(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void AddToList_ShouldReturnExists_WhenProductAlreadyExists()
    {
        // Arrange
        var existingProduct = new Product { ID = "1", Name = "TestProduct" };
        var productList = new List<Product> { existingProduct };
        var json = JsonConvert.SerializeObject(productList);
        _fileServiceMock.Setup(fs => fs.GetFromFile()).Returns(json);

        var newProduct = new Product { ID = "2", Name = "TestProduct" }; // Same name, different ID

        // Act
        var result = _productService.AddToList(newProduct);

        // Assert
        Assert.Equal(ResultStatus.Exists, result);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveExistingProductFromList()
    {
        // Arrange
        var product = new Product { ID = "1", Name = "TestProduct" };
        var productList = new List<Product> { product };
        var json = JsonConvert.SerializeObject(productList);
        _fileServiceMock.Setup(fs => fs.GetFromFile()).Returns(json);
        _fileServiceMock.Setup(fs => fs.SaveToFile(It.IsAny<string>())).Returns(true);

        // Act
        var result = _productService.DeleteProduct(product);

        // Assert
        Assert.Equal(ResultStatus.Success, result);
        _fileServiceMock.Verify(fs => fs.SaveToFile(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productList = new List<Product>
    {
        new Product { ID = "1", Name = "TestProduct" }
    };
        var json = JsonConvert.SerializeObject(productList);
        _fileServiceMock.Setup(fs => fs.GetFromFile()).Returns(json);

        var productToDelete = new Product { ID = "2", Name = "NonExistentProduct" };

        // Act
        var result = _productService.DeleteProduct(productToDelete);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result);
    }

}
