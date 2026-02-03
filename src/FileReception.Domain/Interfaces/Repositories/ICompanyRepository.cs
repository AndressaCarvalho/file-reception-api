using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(int id);
    Task AddAsync(Company company);
}
