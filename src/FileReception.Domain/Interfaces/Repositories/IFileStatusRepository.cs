using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Repositories;

public interface IFileStatusRepository
{
    Task<List<FileStatus>> GetAllAsync();
    Task<FileStatus?> GetByIdAsync(int id);
    Task AddAsync(FileStatus fileStatus);
}
