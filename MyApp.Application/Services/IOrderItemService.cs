using MyApp.Application.DTOs;

namespace MyApp.Application.Services;

public interface IOrderItemService
{
    Task<OrderItemResponseDto> CreateAsync(OrderItemCreateDto dto);
    Task<OrderItemResponseDto> GetByIdAsync(int id);
    Task<List<OrderItemResponseDto>> GetAllAsync();
    Task<OrderItemResponseDto> UpdateAsync(int id, OrderItemUpdateDto dto);
    Task DeleteAsync(int id);
}
