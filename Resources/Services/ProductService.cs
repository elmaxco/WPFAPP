using Newtonsoft.Json;
using Resources.Enums;
using Resources.Models;



namespace Resources.Services;

public class ProductService : IProductService
{
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "file.json");
    private readonly FileService _fileService = new FileService(_filePath);
    private List<Product> _productList = new List<Product>();
    private decimal price;


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


    public ResultStatus DeleteProduct(Product product) // Ändrat från string till Product + fixat stavfel i metodenamn
    {
        try
        {
            // Ladda in produkter från filen
            GetProductsFromFile();

            // Kontrollera om produkten finns i listan
            var productToDelete = _productList.FirstOrDefault(p => p.ID == product.ID);
            if (productToDelete == null)
                return ResultStatus.NotFound;

            // Ta bort produkten från listan
            _productList.Remove(productToDelete);

            // Spara den uppdaterade listan tillbaka till filen
            var json = JsonConvert.SerializeObject(_productList, Newtonsoft.Json.Formatting.Indented);
            var result = _fileService.SaveToFile(json);

            return result ? ResultStatus.Success : ResultStatus.SuccessWithErrors;
        }
        catch
        {
            return ResultStatus.Failed;
        }
    }

    public void GetProductsFromFile()
    {
        try
        {
            var content = _fileService.GetFromFile();

            if (!string.IsNullOrEmpty(content))
                _productList = JsonConvert.DeserializeObject<List<Product>>(content)!;
        }
        catch { }
    }
}