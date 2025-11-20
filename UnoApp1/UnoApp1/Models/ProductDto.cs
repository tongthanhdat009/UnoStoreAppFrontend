namespace UnoApp1.Models;

public partial record ProductDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
}
