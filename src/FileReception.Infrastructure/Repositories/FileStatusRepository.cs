using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Repositories;

public class FileStatusRepository : IFileStatusRepository
{
    private readonly AppDbContext _context;

    public FileStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileStatus>> GetAllAsync()
    {
        return await _context.FileStatus.ToListAsync();
    }

    public async Task<FileStatus?> GetByIdAsync(int id)
    {
        return await _context.FileStatus.FindAsync(id);
    }

    public async Task AddAsync(FileStatus fileStatus)
    {
        await _context.FileStatus.AddAsync(fileStatus);
        await _context.SaveChangesAsync();
    }
}
