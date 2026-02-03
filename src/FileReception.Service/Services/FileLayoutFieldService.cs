using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;

namespace FileReception.Service.Services;

public class FileLayoutFieldService : IFileLayoutFieldService
{
    private readonly IFileLayoutFieldRepository _fileLayoutFieldRepository;

    public FileLayoutFieldService(IFileLayoutFieldRepository fileLayoutFieldRepository)
    {
        _fileLayoutFieldRepository = fileLayoutFieldRepository;
    }

    public async Task<List<FileLayoutField>> GetAllAsync()
    {
        return await _fileLayoutFieldRepository.GetAllAsync();
    }

    public async Task<FileLayoutField> GetByIdAsync(int id)
    {
        var fileLayoutField = await _fileLayoutFieldRepository.GetByIdAsync(id);

        if (fileLayoutField == null)
            throw new KeyNotFoundException("Campos delayout de arquivo não encontrado.");

        return fileLayoutField;
    }

    public async Task<FileLayoutField> AddAsync(FileLayoutField fileLayoutField)
    {
        await _fileLayoutFieldRepository.AddAsync(fileLayoutField);
        
        return fileLayoutField;
    }
}
