using System.Reflection;
using EffectiveMobile.Application;
using EffectiveMobile.Application.AddOrder;
using EffectiveMobile.Application.FilteringOrdersByDistrict;
using EffectiveMobile.Infrastructure;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "EffectiveMobile.Api",
        Contact = new OpenApiContact
        {
            Name = "Alisher",
            Email = "baiyskulov.alisher@gmail.com",
            Url = new Uri("https://t.me/alisher24_8")
        }
    });
    
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

var logFile = builder.Configuration["Data:LogPath"] ?? throw new ArgumentNullException();
if (File.Exists(logFile) == false)
    File.Create(logFile);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(logFile, LogEventLevel.Information)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddScoped<IValidator<AddOrderRequest>, AddOrderValidator>();
builder.Services.AddScoped<IValidator<FilteringOrdersByDistrictRequest>, FilteringOrdersByDistrictValidator>();
builder.Services.AddScoped<IOrderRepository>(_ => new OrderRepository(
    builder.Configuration["Data:OrdersPath"] ?? throw new ArgumentNullException(),
    builder.Configuration["Data:DistrictsPath"] ?? throw new ArgumentNullException(),
    builder.Configuration["Data:DeliveryOrderPath"] ?? throw new ArgumentNullException()));

builder.Services.AddScoped<AddOrderService>();
builder.Services.AddScoped<FilteringOrdersByDistrictService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();