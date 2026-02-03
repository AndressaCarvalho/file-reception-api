using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _context.Company.ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Company.FindAsync(id);
    }

    public async Task AddAsync(Company company)
    {
        await _context.Company.AddAsync(company);
        await _context.SaveChangesAsync();
    }
}
