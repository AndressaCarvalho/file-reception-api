using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Repositories;

public class FileLayoutFieldRepository : IFileLayoutFieldRepository
{
    private readonly AppDbContext _context;

    public FileLayoutFieldRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileLayoutField>> GetAllAsync()
    {
        return await _context.FileLayoutField
                             .Include(flf => flf.FileLayout)
                             .Include(flf => flf.FileLayoutFieldType)
                             .ToListAsync();
    }

    public async Task<FileLayoutField?> GetByIdAsync(int id)
    {
        return await _context.FileLayoutField
                             .Include(flf => flf.FileLayout)
                             .Include(flf => flf.FileLayoutFieldType)
                             .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task AddAsync(FileLayoutField fileLayoutField)
    {
        await _context.FileLayoutField.AddAsync(fileLayoutField);
        await _context.SaveChangesAsync();
    }
}
