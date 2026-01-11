namespace MyApp.Domain.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public string Status { get; set; } = "Created";
    public List<OrderItem> Items { get; set; } = new();
}
