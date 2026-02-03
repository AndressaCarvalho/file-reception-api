using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Services;

public interface IFileLayoutService
{
    Task<List<FileLayout>> GetAllAsync();
    Task<FileLayout> GetByIdAsync(int id);
    Task<FileLayout> AddAsync(FileLayout fileLayout);
}
