namespace UnoApp1.Models;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
}

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<Product>? Products { get; set; }
}

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<Product>? Products { get; set; }
}

