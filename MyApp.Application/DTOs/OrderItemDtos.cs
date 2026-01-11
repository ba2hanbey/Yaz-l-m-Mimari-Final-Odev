namespace MyApp.Application.DTOs;

public record OrderItemCreateDto(int OrderId, int ProductId, int Quantity);
public record OrderItemUpdateDto(int Quantity);

public record OrderItemResponseDto(
    int Id,
    int OrderId,
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
