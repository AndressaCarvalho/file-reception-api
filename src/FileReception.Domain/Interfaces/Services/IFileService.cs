using File = FileReception.Domain.Entities.File;

namespace FileReception.Domain.Interfaces.Services;

public interface IFileService
{
    Task<List<File>> GetAllAsync();
    Task<File> GetByIdAsync(int id);
    Task<Dictionary<string, int>> GetCountByStatusLastDaysAsync(int lastDays);
    Task<File> AddAsync(File file);
}
