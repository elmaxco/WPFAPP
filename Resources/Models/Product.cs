﻿namespace Resources.Models;

public class Product
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string ID { get; set; } = Guid.NewGuid().ToString();
    public string Category { get; set; } = null!; // La till Category för wpf uppgiften
}
