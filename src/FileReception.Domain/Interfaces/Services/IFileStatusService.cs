using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Services;

public interface IFileStatusService
{
    Task<List<FileStatus>> GetAllAsync();
    Task<FileStatus> GetByIdAsync(int id);
    Task<FileStatus> AddAsync(FileStatus fileStatus);
}
