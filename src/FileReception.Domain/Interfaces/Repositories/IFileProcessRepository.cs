using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Repositories;

public interface IFileProcessRepository
{
    Task<List<FileProcess>> GetAllAsync();
    Task<FileProcess?> GetByIdAsync(int id);
    Task AddAsync(FileProcess fileProcess);
}
