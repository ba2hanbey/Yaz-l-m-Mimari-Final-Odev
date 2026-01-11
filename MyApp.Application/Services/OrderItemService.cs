using Microsoft.EntityFrameworkCore;
using MyApp.Application.DTOs;
using MyApp.Application.Exceptions;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly AppDbContext _db;

    public OrderItemService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderItemResponseDto> CreateAsync(OrderItemCreateDto dto)
    {
        if (dto.OrderId <= 0) throw new ValidationException("OrderId must be greater than 0.");
        if (dto.ProductId <= 0) throw new ValidationException("ProductId must be greater than 0.");
        if (dto.Quantity <= 0) throw new ValidationException("Quantity must be greater than 0.");

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == dto.OrderId);
        if (order is null) throw new NotFoundException("Order not found.");

        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
        if (product is null) throw new NotFoundException("Product not found.");

        if (product.Stock < dto.Quantity)
            throw new ConflictException("Not enough stock for this product.");

        
        var entity = new OrderItem
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = product.Price
        };

        
        product.Stock -= dto.Quantity;

        _db.OrderItems.Add(entity);
        await _db.SaveChangesAsync();

        return ToResponse(entity);
    }

    public async Task<List<OrderItemResponseDto>> GetAllAsync()
    {
        var list = await _db.OrderItems.AsNoTracking().ToListAsync();
        return list.Select(ToResponse).ToList();
    }

    public async Task<OrderItemResponseDto> GetByIdAsync(int id)
    {
        var item = await _db.OrderItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) throw new NotFoundException("OrderItem not found.");
        return ToResponse(item);
    }

    public async Task<OrderItemResponseDto> UpdateAsync(int id, OrderItemUpdateDto dto)
    {
        if (dto.Quantity <= 0) throw new ValidationException("Quantity must be greater than 0.");

        var item = await _db.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) throw new NotFoundException("OrderItem not found.");

        
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
        if (product is null) throw new NotFoundException("Product not found.");

        var diff = dto.Quantity - item.Quantity; 
        if (diff > 0 && product.Stock < diff)
            throw new ConflictException("Not enough stock for this update.");

        product.Stock -= diff; 
        item.Quantity = dto.Quantity;

        await _db.SaveChangesAsync();
        return ToResponse(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _db.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) throw new NotFoundException("OrderItem not found.");

        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
        if (product is not null)
        {
            
            product.Stock += item.Quantity;
        }

        _db.OrderItems.Remove(item);
        await _db.SaveChangesAsync();
    }

    private static OrderItemResponseDto ToResponse(OrderItem oi)
        => new(oi.Id, oi.OrderId, oi.ProductId, oi.Quantity, oi.UnitPrice, oi.CreatedAt, oi.UpdatedAt);
}
