global using EnergyManagementSystem.Models;
using EnergyManagementSystem.Data;
using EnergyManagementSystem.Services;
using EnergyManagementSystem.Services.DeviceService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DeviceDataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DevicesDB"))  
);

builder.Services.AddScoped<IDeviceService, DeviceService>();


builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();

    });
});

builder.Services.AddSingleton<IRabbitMQPublisher>(sp =>
{
    string rabbitMQUri = builder.Configuration["RabbitMQ:Uri"];
    string exchangeName = builder.Configuration["RabbitMQ:ExchangeName"];
    string routingKey = builder.Configuration["RabbitMQ:RoutingKey"];
    string queueName = builder.Configuration["RabbitMQ:QueueName"];

    return new RabbitMQPublisher(rabbitMQUri, exchangeName, routingKey,queueName);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
