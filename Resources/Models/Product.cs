namespace Resources.Models;

public class Product
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string ID { get; set; } = Guid.NewGuid().ToString();

}
