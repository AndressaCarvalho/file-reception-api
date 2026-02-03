using File = FileReception.Domain.Entities.File;

namespace FileReception.Domain.Interfaces.Repositories;

public interface IFileRepository
{
    Task<List<File>> GetAllAsync();
    Task<File?> GetByIdAsync(int id);
    Task<File?> GetByNameAndStatusAsync(string fileName, int status);
    Task<Dictionary<string, int>> GetCountByStatusLastDaysAsync(DateTime fromDate);
    Task AddAsync(File file);
    Task UpdateAsync(File file);

}
