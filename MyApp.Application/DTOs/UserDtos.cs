namespace MyApp.Application.DTOs;

public record UserCreateDto(string Email, string FullName);
public record UserUpdateDto(string FullName);
public record UserResponseDto(int Id, string Email, string FullName, DateTime CreatedAt, DateTime UpdatedAt);
