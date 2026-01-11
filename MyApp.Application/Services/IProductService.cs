using MyApp.Application.DTOs;

namespace MyApp.Application.Services;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(ProductCreateDto dto);
    Task<ProductResponseDto> GetByIdAsync(int id);
    Task<List<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto> UpdateAsync(int id, ProductUpdateDto dto);
    Task DeleteAsync(int id);
}
