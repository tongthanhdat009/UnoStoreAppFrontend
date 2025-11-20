namespace UnoApp1.Models;

/// <summary>
/// Model cho sản phẩm trong giỏ hàng, lưu trữ offline trong SQLite
/// </summary>
public class CartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Tính tổng tiền cho item này
    /// </summary>
    public decimal Subtotal => Price * Quantity;
}

