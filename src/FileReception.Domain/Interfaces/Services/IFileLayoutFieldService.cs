using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Services;

public interface IFileLayoutFieldService
{
    Task<List<FileLayoutField>> GetAllAsync();
    Task<FileLayoutField> GetByIdAsync(int id);
    Task<FileLayoutField> AddAsync(FileLayoutField fileLayoutField);
}
