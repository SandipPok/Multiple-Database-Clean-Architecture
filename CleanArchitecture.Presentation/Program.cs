using CleanArchitecture.Application.Interface;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddScoped<DbContext>(provider => 
    new DbContext(defaultConnectionString ?? ""));

builder.Services.AddScoped<MySqlDbContext>(provider =>
    new MySqlDbContext(mySqlConnectionString ?? ""));

builder.Services.AddTransient<IItemRepository, ItemRepository>();
builder.Services.AddTransient<IItemDetailsRepository, ItemDetailsRepository>();
builder.Services.AddTransient<IItemDetailsBackupRepository, ItemDetailsBackupRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
