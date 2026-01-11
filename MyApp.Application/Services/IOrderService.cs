using MyApp.Application.DTOs;

namespace MyApp.Application.Services;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(OrderCreateDto dto);
    Task<OrderResponseDto> GetByIdAsync(int id);
    Task<List<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto> UpdateAsync(int id, OrderUpdateDto dto);
    Task DeleteAsync(int id);
}
