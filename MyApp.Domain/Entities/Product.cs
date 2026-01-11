namespace MyApp.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}
