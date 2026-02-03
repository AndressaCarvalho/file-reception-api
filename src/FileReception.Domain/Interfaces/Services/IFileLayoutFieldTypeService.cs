using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Services;

public interface IFileLayoutFieldTypeService
{
    Task<List<FileLayoutFieldType>> GetAllAsync();
    Task<FileLayoutFieldType> GetByIdAsync(int id);
    Task<FileLayoutFieldType> AddAsync(FileLayoutFieldType fileLayoutFieldType);
}
