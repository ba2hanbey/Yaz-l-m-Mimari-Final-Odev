using Microsoft.EntityFrameworkCore;
using MyApp.Api.Middleware;
using MyApp.Application.DTOs;
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
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();


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





var products = app.MapGroup("/products");

products.MapPost("/", async (ProductCreateDto dto, IProductService service) =>
{
    var result = await service.CreateAsync(dto);
    return Results.Created($"/products/{result.Id}", ApiResponse<object>.Ok(result, "Product created"));
});

products.MapGet("/", async (IProductService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(result, "Products listed"));
});

products.MapGet("/{id:int}", async (int id, IProductService service) =>
{
    var result = await service.GetByIdAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(result, "Product found"));
});

products.MapPut("/{id:int}", async (int id, ProductUpdateDto dto, IProductService service) =>
{
    var result = await service.UpdateAsync(id, dto);
    return Results.Ok(ApiResponse<object>.Ok(result, "Product updated"));
});

products.MapDelete("/{id:int}", async (int id, IProductService service) =>
{
    await service.DeleteAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(new { }, "Product deleted"));
});




var orders = app.MapGroup("/orders");

orders.MapPost("/", async (OrderCreateDto dto, IOrderService service) =>
{
    var result = await service.CreateAsync(dto);
    return Results.Created($"/orders/{result.Id}", ApiResponse<object>.Ok(result, "Order created"));
});

orders.MapGet("/", async (IOrderService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(result, "Orders listed"));
});

orders.MapGet("/{id:int}", async (int id, IOrderService service) =>
{
    var result = await service.GetByIdAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(result, "Order found"));
});

orders.MapPut("/{id:int}", async (int id, OrderUpdateDto dto, IOrderService service) =>
{
    var result = await service.UpdateAsync(id, dto);
    return Results.Ok(ApiResponse<object>.Ok(result, "Order updated"));
});

orders.MapDelete("/{id:int}", async (int id, IOrderService service) =>
{
    await service.DeleteAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(new { }, "Order deleted"));
});




var orderItems = app.MapGroup("/order-items");

orderItems.MapPost("/", async (OrderItemCreateDto dto, IOrderItemService service) =>
{
    var result = await service.CreateAsync(dto);
    return Results.Created($"/order-items/{result.Id}", ApiResponse<object>.Ok(result, "OrderItem created"));
});

orderItems.MapGet("/", async (IOrderItemService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(result, "OrderItems listed"));
});

orderItems.MapGet("/{id:int}", async (int id, IOrderItemService service) =>
{
    var result = await service.GetByIdAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(result, "OrderItem found"));
});

orderItems.MapPut("/{id:int}", async (int id, OrderItemUpdateDto dto, IOrderItemService service) =>
{
    var result = await service.UpdateAsync(id, dto);
    return Results.Ok(ApiResponse<object>.Ok(result, "OrderItem updated"));
});

orderItems.MapDelete("/{id:int}", async (int id, IOrderItemService service) =>
{
    await service.DeleteAsync(id);
    return Results.Ok(ApiResponse<object>.Ok(new { }, "OrderItem deleted"));
});









app.Run();



