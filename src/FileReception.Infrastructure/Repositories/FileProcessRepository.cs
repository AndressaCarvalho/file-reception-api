using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Repositories;

public class FileProcessRepository : IFileProcessRepository
{
    private readonly AppDbContext _context;

    public FileProcessRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileProcess>> GetAllAsync()
    {
        return await _context.FileProcess
                             .Include(fp => fp.File)
                             .ToListAsync();
    }

    public async Task<FileProcess?> GetByIdAsync(int id)
    {
        return await _context.FileProcess
                            .Include(fp => fp.File)
                            .FirstOrDefaultAsync(fp => fp.Id == id);
    }

    public async Task AddAsync(FileProcess fileProcess)
    {
        await _context.Set<FileProcess>().AddAsync(fileProcess);
        await _context.SaveChangesAsync();
    }
}
