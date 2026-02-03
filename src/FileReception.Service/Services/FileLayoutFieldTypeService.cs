using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;

namespace FileReception.Service.Services;

public class FileLayoutFieldTypeService : IFileLayoutFieldTypeService
{
    private readonly IFileLayoutFieldTypeRepository _fileLayoutFieldTypeRepository;

    public FileLayoutFieldTypeService(IFileLayoutFieldTypeRepository fileLayoutFieldTypeRepository)
    {
        _fileLayoutFieldTypeRepository = fileLayoutFieldTypeRepository;
    }

    public async Task<List<FileLayoutFieldType>> GetAllAsync()
    {
        return await _fileLayoutFieldTypeRepository.GetAllAsync();
    }

    public async Task<FileLayoutFieldType> GetByIdAsync(int id)
    {
        var fileLayoutFieldType = await _fileLayoutFieldTypeRepository.GetByIdAsync(id);

        if (fileLayoutFieldType == null)
            throw new KeyNotFoundException("Tipo de campo de layout não encontrado.");

        return fileLayoutFieldType;
    }

    public async Task<FileLayoutFieldType> AddAsync(FileLayoutFieldType fileLayoutFieldType)
    {
        await _fileLayoutFieldTypeRepository.AddAsync(fileLayoutFieldType);
        
        return fileLayoutFieldType;
    }
}
