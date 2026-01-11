using MyApp.Application.DTOs;

namespace MyApp.Application.Services;

public interface IUserService
{
    Task<UserResponseDto> CreateAsync(UserCreateDto dto);
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<List<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> UpdateAsync(int id, UserUpdateDto dto);
    Task DeleteAsync(int id);
}
