using Resources.Enums;
using Resources.Models;

namespace Resources.Services
{
    public interface IProductService
    {
        ResultStatus AddToList(Product product);
        ResultStatus DeleteProduct(Product product);
        IEnumerable<Product> GetAllProducts();
        void GetProductsFromFile();
    }
}