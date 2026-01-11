namespace MyApp.Application.DTOs;

public record OrderCreateDto(int UserId, string Status);
public record OrderUpdateDto(string Status);

public record OrderResponseDto(
    int Id,
    int UserId,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
