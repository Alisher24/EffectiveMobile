using EffectiveMobile.Application;
using EffectiveMobile.Application.AddOrder;
using EffectiveMobile.Infrastructure;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<AddOrderRequest>, AddOrderValidator>();
builder.Services.AddScoped<IOrderRepository>(_ =>
    new OrderRepository(builder.Configuration["Orders:FileName"] ?? throw new ArgumentNullException()));
builder.Services.AddScoped<AddOrderService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();