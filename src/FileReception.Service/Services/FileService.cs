using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;
using File = FileReception.Domain.Entities.File;

namespace FileReception.Service.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<List<File>> GetAllAsync()
    {
        return await _fileRepository.GetAllAsync();
    }

    public async Task<File> GetByIdAsync(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);

        if (file == null)
            throw new KeyNotFoundException("Arquivo não encontrado.");

        return file;
    }

    public async Task<Dictionary<string, int>> GetCountByStatusLastDaysAsync(int lastDays)
    {
        if (lastDays <= 0)
            throw new KeyNotFoundException("Informe um número de dias válido.");

        var fromDate = DateTime.Now.AddDays(-lastDays);

        return await _fileRepository.GetCountByStatusLastDaysAsync(fromDate);
    }

    public async Task<File> AddAsync(File file)
    {
        await _fileRepository.AddAsync(file);
        
        return file;
    }
}
