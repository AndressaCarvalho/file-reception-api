using FileReception.Domain.Entities;

namespace FileReception.Domain.Interfaces.Services;

public interface ICompanyService
{
    Task<List<Company>> GetAllAsync();
    Task<Company> GetByIdAsync(int id);
    Task<Company> AddAsync(Company company);
}
