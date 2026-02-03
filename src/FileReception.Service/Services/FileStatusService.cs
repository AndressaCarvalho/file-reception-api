using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;

namespace FileReception.Service.Services;

public class FileStatusService : IFileStatusService
{
    private readonly IFileStatusRepository _fileStatusRepository;

    public FileStatusService(IFileStatusRepository fileStatusRepository)
    {
        _fileStatusRepository = fileStatusRepository;
    }

    public async Task<List<FileStatus>> GetAllAsync()
    {
        return await _fileStatusRepository.GetAllAsync();
    }

    public async Task<FileStatus> GetByIdAsync(int id)
    {
        var fileStatus = await _fileStatusRepository.GetByIdAsync(id);

        if (fileStatus == null)
            throw new KeyNotFoundException("Status não encontrado.");

        return fileStatus;
    }

    public async Task<FileStatus> AddAsync(FileStatus fileStatus)
    {
        await _fileStatusRepository.AddAsync(fileStatus);

        return fileStatus;
    }
}
