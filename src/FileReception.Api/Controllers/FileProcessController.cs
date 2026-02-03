using FileReception.Api.Filters.Cache;
using FileReception.Api.Filters.Cache.Helpers;
using FileReception.Api.Models.Responses;
using FileReception.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FileReception.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileProcessController : ControllerBase
{
    private readonly IFileProcessService _fileProcessService;
    private readonly IDatabase _redisDb;
    private readonly IConfiguration _configuration;

    public FileProcessController(IFileProcessService fileProcessService, IConnectionMultiplexer redis, IConfiguration configuration)
    {
        _fileProcessService = fileProcessService;
        _redisDb = redis.GetDatabase();
        _configuration = configuration;
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var fileProcesses = await _fileProcessService.GetAllAsync();

        return Ok(fileProcesses.Select(fp => new FileProcessResponse
        {
            Id = fp.Id,
            FileId = fp.File.Id,
            FileName = fp.File.Name,
            FilePathBackup = fp.FilePathBackup,
            ReceivedAt = fp.ReceivedAt,
            IsValid = fp.IsValid,
            ErrorMessage = fp.ErrorMessage
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var fileProcess = await _fileProcessService.GetByIdAsync(id);

        return Ok(new FileProcessResponse
        {
            Id = fileProcess.Id,
            FileId = fileProcess.File.Id,
            FileName = fileProcess.File.Name,
            FilePathBackup = fileProcess.FilePathBackup,
            ReceivedAt = fileProcess.ReceivedAt,
            IsValid = fileProcess.IsValid,
            ErrorMessage = fileProcess.ErrorMessage
        });
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync([FromForm] IFormFile file)
    {
        var fileProcessCreated = await _fileProcessService.ProcessFileAsync(file);

        await CleanCache();

        fileProcessCreated = await _fileProcessService.GetByIdAsync(fileProcessCreated.Id);

        return Ok(new FileProcessResponse
        {
            Id = fileProcessCreated.Id,
            FileId = fileProcessCreated.File.Id,
            FileName = fileProcessCreated.File.Name,
            FilePathBackup = fileProcessCreated.FilePathBackup,
            ReceivedAt = fileProcessCreated.ReceivedAt,
            IsValid = fileProcessCreated.IsValid,
            ErrorMessage = fileProcessCreated.ErrorMessage
        });
    }

    private async Task CleanCache()
    {
        var reportLastDays = _configuration.GetSection("FileConfig").GetSection("Report").GetSection("LastDays");
        var lastDaysField = reportLastDays["FieldName"];
        var lastDaysValue = reportLastDays["FieldValue"];

        await CacheHelper.RemoveCacheAsync(typeof(FileController), _redisDb);
        await CacheHelper.RemoveCacheAsync(typeof(FileController), _redisDb, $"report?{lastDaysField}={lastDaysValue}");
        await CacheHelper.RemoveCacheAsync(typeof(FileProcessController), _redisDb);
    }
}
