using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Repositories;

public interface IFileLayoutFieldRepository
{
    Task<List<FileLayoutField>> GetAllAsync();
    Task<FileLayoutField?> GetByIdAsync(int id);
    Task AddAsync(FileLayoutField fileLayoutField);
}
