namespace MyApp.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public List<Order> Orders { get; set; } = new();
}
