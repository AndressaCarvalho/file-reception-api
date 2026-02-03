using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Repositories;

public interface IFileLayoutRepository
{
    Task<List<FileLayout>> GetAllAsync();
    Task<FileLayout?> GetByIdAsync(int id);
    Task<FileLayout?> GetByIdWithFieldsAsync(int id);
    Task AddAsync(FileLayout fileLayout);

}
