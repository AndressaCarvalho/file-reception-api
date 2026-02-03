using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;

namespace FileReception.Service.Services;

public class FileLayoutService : IFileLayoutService
{
    private readonly IFileLayoutRepository _fileLayoutRepository;

    public FileLayoutService(IFileLayoutRepository fileLayoutRepository)
    {
        _fileLayoutRepository = fileLayoutRepository;
    }

    public async Task<List<FileLayout>> GetAllAsync()
    {
        return await _fileLayoutRepository.GetAllAsync();
    }

    public async Task<FileLayout> GetByIdAsync(int id)
    {
        var fileLayout = await _fileLayoutRepository.GetByIdAsync(id);

        if (fileLayout == null)
            throw new KeyNotFoundException("Layout de arquivo não encontrado.");

        return fileLayout;
    }

    public async Task<FileLayout> AddAsync(FileLayout fileLayout)
    {
        await _fileLayoutRepository.AddAsync(fileLayout);
        
        return fileLayout;
    }
}
