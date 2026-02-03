using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;

namespace FileReception.Service.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _companyRepository.GetAllAsync();
    }

    public async Task<Company> GetByIdAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new KeyNotFoundException("Companhia não encontrada.");

        return company;
    }

    public async Task<Company> AddAsync(Company company)
    {
        await _companyRepository.AddAsync(company);
        
        return company;
    }
}
