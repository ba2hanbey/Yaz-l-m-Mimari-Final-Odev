namespace MyApp.Application.DTOs;

public record ProductCreateDto(string Name, decimal Price, int Stock);
public record ProductUpdateDto(string Name, decimal Price, int Stock);
public record ProductResponseDto(int Id, string Name, decimal Price, int Stock, DateTime CreatedAt, DateTime UpdatedAt);
