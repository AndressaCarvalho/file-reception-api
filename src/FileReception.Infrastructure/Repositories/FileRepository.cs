using FileReception.Domain.Interfaces.Repositories;
using FileReception.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using File = FileReception.Domain.Entities.File;

namespace FileReception.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;

    public FileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<File>> GetAllAsync()
    {
        return await _context.File
                             .Include(f => f.FileLayout)
                                .ThenInclude(fl => fl.Company)
                             .Include(f => f.FileStatus)
                             .ToListAsync();
    }

    public async Task<File?> GetByIdAsync(int id)
    {
        return await _context.File
                             .Include(f => f.FileLayout)
                                .ThenInclude(fl => fl.Company)
                             .Include(f => f.FileStatus)
                             .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<File?> GetByNameAndStatusAsync(string fileName, int status)
    {
        var normalizedFileName = fileName.ToUpper();

        return await _context.File
                            .Include(f => f.FileLayout)
                                .ThenInclude(fl => fl.Company)
                            .Include(f => f.FileStatus)
                            .FirstOrDefaultAsync(f =>
                                f.Name.ToUpper() == normalizedFileName &&
                                f.FileStatus.Id == status
                            );
    }

    public async Task<Dictionary<string, int>> GetCountByStatusLastDaysAsync(DateTime fromDate)
    {
        var allStatuses = await _context.FileStatus
                                        .Select(fs => fs.Name)
                                        .ToListAsync();

        var query = await _context.File
                                .Where(f => f.ExpectedDate >= fromDate)
                                .GroupBy(f => f.FileStatus.Name)
                                .Select(g => new { Status = g.Key, Count = g.Count() })
                                .ToListAsync();

        var result = allStatuses.ToDictionary(
            status => status,
            status => query.FirstOrDefault(q => q.Status == status)?.Count ?? 0
        );

        return result;
    }

    public async Task AddAsync(File file)
    {
        await _context.File.AddAsync(file);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(File file)
    {
        _context.File.Update(file);
        await _context.SaveChangesAsync();
    }
}
