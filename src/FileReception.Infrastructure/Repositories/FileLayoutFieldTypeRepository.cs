using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Repositories;

public class FileLayoutFieldTypeRepository : IFileLayoutFieldTypeRepository
{
    private readonly AppDbContext _context;

    public FileLayoutFieldTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileLayoutFieldType>> GetAllAsync()
    {
        return await _context.FileLayoutFieldType.ToListAsync();
    }

    public async Task<FileLayoutFieldType?> GetByIdAsync(int id)
    {
        return await _context.FileLayoutFieldType.FindAsync(id);
    }

    public async Task AddAsync(FileLayoutFieldType fileLayoutFieldType)
    {
        await _context.FileLayoutFieldType.AddAsync(fileLayoutFieldType);
        await _context.SaveChangesAsync();
    }
}
