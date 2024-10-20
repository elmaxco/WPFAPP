using Newtonsoft.Json;
using Resources.Enums;
using Resources.Models;

namespace Resources.Services;

public class ProductService : IProductService
{
    private readonly IFileService _fileService;
    private List<Product> _productList = new List<Product>();

    public ProductService(IFileService fileService) // Uppdaterad konstruktor
    {
        _fileService = fileService;
    }

    public ResultStatus AddToList(Product product)
    {
        try
        {
            GetProductsFromFile();

            if (_productList.Any(c => c.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
                return ResultStatus.Exists;

            _productList.Add(product);

            var json = JsonConvert.SerializeObject(_productList, Newtonsoft.Json.Formatting.Indented);
            var result = _fileService.SaveToFile(json);
            if (result)
                return ResultStatus.Success;

            return ResultStatus.SuccessWithErrors;
        }
        catch
        {
            return ResultStatus.Failed;
        }
    }

    public IEnumerable<Product> GetAllProducts()
    {
        GetProductsFromFile();
        return _productList;
    }

    public ResultStatus DeleteProduct(Product product)
    {
        try
        {
            GetProductsFromFile();

            var productToDelete = _productList.FirstOrDefault(p => p.ID == product.ID);
            if (productToDelete == null)
                return ResultStatus.NotFound;

            _productList.Remove(productToDelete);

            var json = JsonConvert.SerializeObject(_productList, Newtonsoft.Json.Formatting.Indented);
            var result = _fileService.SaveToFile(json);

            return result ? ResultStatus.Success : ResultStatus.SuccessWithErrors;
        }
        catch
        {
            return ResultStatus.Failed;
        }
    }

    // Metoden är nu ändrad till public för att matcha gränssnittet
    public void GetProductsFromFile()
    {
        try
        {
            var content = _fileService.GetFromFile();

            if (!string.IsNullOrEmpty(content))
                _productList = JsonConvert.DeserializeObject<List<Product>>(content)!;
        }
        catch
        {
            
        }
    }
}
