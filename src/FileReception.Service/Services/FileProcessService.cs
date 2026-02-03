using FileReception.Domain.Entities;
using FileReception.Domain.Enums;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Domain.Interfaces.Services;
using FileReception.Service.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileReception.Service.Services;

public class FileProcessService : IFileProcessService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileProcessRepository _fileProcessRepository;
    private readonly IFileLayoutRepository _fileLayoutRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileService> _logger;

    private readonly string _backupPath;

    public FileProcessService(IFileRepository fileRepository, 
        IFileProcessRepository fileProcessRepository,
        IFileLayoutRepository fileLayoutRepository,
        IConfiguration configuration,
        ILogger<FileService> logger)
    {
        _fileRepository = fileRepository;
        _fileProcessRepository = fileProcessRepository;
        _fileLayoutRepository = fileLayoutRepository;
        _configuration = configuration;
        _logger = logger;

        _backupPath = _configuration.GetSection("FileConfig")["BackupPath"] ?? "C:\\FileBackups";
    }

    public async Task<List<FileProcess>> GetAllAsync()
    {
        return await _fileProcessRepository.GetAllAsync();
    }

    public async Task<FileProcess> GetByIdAsync(int id)
    {
        var fileProcess = await _fileProcessRepository.GetByIdAsync(id);

        if (fileProcess == null)
            throw new KeyNotFoundException("Processamento de arquivo não encontrado.");

        return fileProcess;
    }

    public async Task<FileProcess> ProcessFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Arquivo inválido.");

        _logger.LogInformation("Recebendo arquivo {FileName}.", file.FileName);

        var fileLines = await FileHelper.ReadFileLinesAsync(file);

        var expectedFile = await _fileRepository.GetByNameAndStatusAsync(file.FileName, (int)Status.NotReceived);

        if (expectedFile == null)
            throw new InvalidOperationException("Arquivo não esperado.");

        var layout = await _fileLayoutRepository.GetByIdWithFieldsAsync(expectedFile.FileLayoutId);

        if (layout == null)
            throw new InvalidOperationException("Layout não encontrado.");

        var (isValid, errorMessage) = FileHelper.ValidateLines(fileLines, layout);

        var backupFilePath = await FileHelper.BackupFileAsync(file, _backupPath, _logger);

        var fileProcess = new FileProcess
        {
            FileId = expectedFile.Id,
            ReceivedAt = DateTime.Now,
            FilePathBackup = backupFilePath,
            IsValid = isValid,
            ErrorMessage = errorMessage
        };

        await _fileProcessRepository.AddAsync(fileProcess);

        expectedFile.StatusId = isValid ? (int)Status.Received : (int)Status.Error;

        await _fileRepository.UpdateAsync(expectedFile);

        _logger.LogInformation("Arquivo processado: {FileName}; Válido: {IsValid}.", file.FileName, isValid);

        return fileProcess;
    }
}
