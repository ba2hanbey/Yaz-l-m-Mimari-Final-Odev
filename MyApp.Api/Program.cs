using Microsoft.EntityFrameworkCore;
using MyApp.Api.Middleware;
using MyApp.Application.Common;
using MyApp.Application.Services;
using MyApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlite(cs);
});


builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var users = app.MapGroup("/users");

users.MapPost("/", async (UserCreateDto dto, IUserService service) =>
{
    var result = await service.CreateAsync(dto);
    return Results.Created($"/users/{result.Id}", ApiResponse<object>.Ok(result, "User created"));
});

users.MapGet("/", async (IUserService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(result, "Users listed"));
});

users.MapGet("/{id:int}", async (int id, IUserService service) =>
{
    var result = await service.GetByIdAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(result, "User found"));
});

users.MapPut("/{id:int}", async (int id, UserUpdateDto dto, IUserService service) =>
{
    var result = await service.UpdateAsync(id, dto);
    return Results.Ok(ApiResponse<object>.Ok(result, "User updated"));
});

users.MapDelete("/{id:int}", async (int id, IUserService service) =>
{
    await service.DeleteAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(new { }, "User deleted"));
});

app.Run();


using MyApp.Application.DTOs;
