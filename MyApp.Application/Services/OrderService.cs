using Microsoft.EntityFrameworkCore;
using MyApp.Application.DTOs;
using MyApp.Application.Exceptions;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Application.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderResponseDto> CreateAsync(OrderCreateDto dto)
    {
        if (dto.UserId <= 0)
            throw new ValidationException("UserId must be greater than 0.");

        if (string.IsNullOrWhiteSpace(dto.Status))
            throw new ValidationException("Status is required.");

        var userExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists)
            throw new NotFoundException("User not found.");

        var entity = new Order
        {
            UserId = dto.UserId,
            Status = dto.Status.Trim()
        };

        _db.Orders.Add(entity);
        await _db.SaveChangesAsync();

        return ToResponse(entity);
    }

    public async Task<List<OrderResponseDto>> GetAllAsync()
    {
        var list = await _db.Orders.AsNoTracking().ToListAsync();
        return list.Select(ToResponse).ToList();
    }

    public async Task<OrderResponseDto> GetByIdAsync(int id)
    {
        var order = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
            throw new NotFoundException("Order not found.");

        return ToResponse(order);
    }

    public async Task<OrderResponseDto> UpdateAsync(int id, OrderUpdateDto dto)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
            throw new NotFoundException("Order not found.");

        if (string.IsNullOrWhiteSpace(dto.Status))
            throw new ValidationException("Status is required.");

        order.Status = dto.Status.Trim();
        await _db.SaveChangesAsync();

        return ToResponse(order);
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
            throw new NotFoundException("Order not found.");

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
    }

    private static OrderResponseDto ToResponse(Order o)
        => new(o.Id, o.UserId, o.Status, o.CreatedAt, o.UpdatedAt);
}
