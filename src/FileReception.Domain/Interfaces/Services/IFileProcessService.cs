using FileReception.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FileReception.Domain.Interfaces.Services;

public interface IFileProcessService
{
    Task<List<FileProcess>> GetAllAsync();
    Task<FileProcess> GetByIdAsync(int id);
    Task<FileProcess> ProcessFileAsync(IFormFile file);
}
