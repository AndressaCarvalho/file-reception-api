using FileReception.Api.Extensions;
using FileReception.Api.Middlewares;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;
using FileReception.Infrastructure.Contexts;
using FileReception.Infrastructure.Repositories;
using FileReception.Service.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("Configurations/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"Configurations/appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IFileLayoutRepository, FileLayoutRepository>();
builder.Services.AddScoped<IFileLayoutFieldTypeRepository, FileLayoutFieldTypeRepository>();
builder.Services.AddScoped<IFileLayoutFieldRepository, FileLayoutFieldRepository>();
builder.Services.AddScoped<IFileStatusRepository, FileStatusRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileProcessRepository, FileProcessRepository>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IFileLayoutService, FileLayoutService>();
builder.Services.AddScoped<IFileLayoutFieldTypeService, FileLayoutFieldTypeService>();
builder.Services.AddScoped<IFileLayoutFieldService, FileLayoutFieldService>();
builder.Services.AddScoped<IFileStatusService, FileStatusService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileProcessService, FileProcessService>();

var redisConfiguration = builder.Configuration.GetSection("Redis:Configuration").Value
                        ?? throw new InvalidOperationException("Configuração do Redis não encontrada.");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.ApplyMigrations<AppDbContext>();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandler>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
