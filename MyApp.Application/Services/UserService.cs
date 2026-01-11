using Microsoft.EntityFrameworkCore;
using MyApp.Application.DTOs;
using MyApp.Application.Exceptions;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidationException("Email is required.");

        var exists = await _db.Users.AnyAsync(x => x.Email == dto.Email);
        if (exists)
            throw new ConflictException("User with this email already exists.");

        var entity = new User
        {
            Email = dto.Email.Trim(),
            FullName = dto.FullName.Trim()
        };

        _db.Users.Add(entity);
        await _db.SaveChangesAsync();

        return ToResponse(entity);
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var list = await _db.Users.AsNoTracking().ToListAsync();
        return list.Select(ToResponse).ToList();
    }

    public async Task<UserResponseDto> GetByIdAsync(int id)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (user is null) throw new NotFoundException("User not found.");
        return ToResponse(user);
    }

    public async Task<UserResponseDto> UpdateAsync(int id, UserUpdateDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null) throw new NotFoundException("User not found.");

        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ValidationException("FullName is required.");

        user.FullName = dto.FullName.Trim();
        await _db.SaveChangesAsync();

        return ToResponse(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null) throw new NotFoundException("User not found.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }

    private static UserResponseDto ToResponse(User u)
        => new(u.Id, u.Email, u.FullName, u.CreatedAt, u.UpdatedAt);
}
