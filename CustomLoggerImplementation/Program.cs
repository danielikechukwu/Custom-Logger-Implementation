using CustomLoggerImplementation.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core for SQL Server logging.
builder.Services.AddDbContext<LoggingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LoggingDBConnection")));

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

// --- Register the Custom Logger Provider ---
// Define the file path where logs will be stored.
string logFilePath = "Logs/logs.txt";

// Retrieve the IServiceScopeFactory from the app's services.
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

// Add our custom logger provider to the logging factory.
// You can choose the minimum log level you want (e.g., LogLevel.Information).
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddProvider(new CustomLoggerProvider(logFilePath, LogLevel.Information, scopeFactory));

app.Run();
