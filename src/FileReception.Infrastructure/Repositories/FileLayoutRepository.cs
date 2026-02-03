using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Repositories;

public class FileLayoutRepository : IFileLayoutRepository
{
    private readonly AppDbContext _context;

    public FileLayoutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileLayout>> GetAllAsync()
    {
        return await _context.FileLayout
                             .Include(fl => fl.Company)
                             .ToListAsync();
    }

    public async Task<FileLayout?> GetByIdAsync(int id)
    {
        return await _context.FileLayout
                             .Include(fl => fl.Company)
                             .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<FileLayout?> GetByIdWithFieldsAsync(int id)
    {
        return await _context.FileLayout
                            .Include(fl => fl.Company)
                            .Include(fl => fl.Fields)
                                .ThenInclude(f => f.FileLayoutFieldType)
                            .FirstOrDefaultAsync(fl => fl.Id == id);
    }

    public async Task AddAsync(FileLayout fileLayout)
    {
        await _context.FileLayout.AddAsync(fileLayout);
        await _context.SaveChangesAsync();
    }
}
