using Microsoft.EntityFrameworkCore;
using MyApp.Application.DTOs;
using MyApp.Application.Exceptions;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Application.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    public ProductService(AppDbContext db) => _db = db;

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Product name is required.");
        if (dto.Price < 0) throw new ValidationException("Price cannot be negative.");
        if (dto.Stock < 0) throw new ValidationException("Stock cannot be negative.");

        var exists = await _db.Products.AnyAsync(p => p.Name == dto.Name.Trim());
        if (exists) throw new ConflictException("Product with this name already exists.");

        var entity = new Product
        {
            Name = dto.Name.Trim(),
            Price = dto.Price,
            Stock = dto.Stock
        };

        _db.Products.Add(entity);
        await _db.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var list = await _db.Products.AsNoTracking().ToListAsync();
        return list.Select(ToResponse).ToList();
    }

    public async Task<ProductResponseDto> GetByIdAsync(int id)
    {
        var p = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) throw new NotFoundException("Product not found.");
        return ToResponse(p);
    }

    public async Task<ProductResponseDto> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) throw new NotFoundException("Product not found.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Product name is required.");
        if (dto.Price < 0) throw new ValidationException("Price cannot be negative.");
        if (dto.Stock < 0) throw new ValidationException("Stock cannot be negative.");

        var newName = dto.Name.Trim();
        var nameConflict = await _db.Products.AnyAsync(x => x.Id != id && x.Name == newName);
        if (nameConflict) throw new ConflictException("Another product with this name already exists.");

        p.Name = newName;
        p.Price = dto.Price;
        p.Stock = dto.Stock;

        await _db.SaveChangesAsync();
        return ToResponse(p);
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) throw new NotFoundException("Product not found.");

        _db.Products.Remove(p);
        await _db.SaveChangesAsync();
    }

    private static ProductResponseDto ToResponse(Product p)
        => new(p.Id, p.Name, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt);
}
