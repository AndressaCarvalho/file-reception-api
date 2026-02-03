using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Repositories;

public interface IFileLayoutFieldTypeRepository
{
    Task<List<FileLayoutFieldType>> GetAllAsync();
    Task<FileLayoutFieldType?> GetByIdAsync(int id);
    Task AddAsync(FileLayoutFieldType fileLayoutFieldType);
}
