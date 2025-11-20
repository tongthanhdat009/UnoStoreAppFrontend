namespace UnoApp1.Models;

public class OrderModel
{
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public int? PromoId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal { get; set; }
}

public class Payment
{
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}

