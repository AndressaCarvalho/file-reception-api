using FileReception.Api.Filters.Cache;
using FileReception.Api.Filters.Cache.Helpers;
using FileReception.Api.Models.Requests;
using FileReception.Api.Models.Responses;
using FileReception.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using File = FileReception.Domain.Entities.File;

namespace FileReception.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IDatabase _redisDb;
    private readonly IConfiguration _configuration;

    public FileController(IFileService fileService, IConnectionMultiplexer redis, IConfiguration configuration)
    {
        _fileService = fileService;
        _redisDb = redis.GetDatabase();
        _configuration = configuration;
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var files = await _fileService.GetAllAsync();

        return Ok(files.Select(f => new FileResponse
        {
            Id = f.Id,
            FileLayoutId = f.FileLayout.Id,
            FileLayoutName = f.FileLayout.Name,
            CompanyId = f.FileLayout.Company.Id,
            CompanyName = f.FileLayout.Company.Name,
            FileName = f.Name,
            ExpectedDate = f.ExpectedDate,
            StatusId = f.FileStatus.Id,
            StatusName = f.FileStatus.Name
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var file = await _fileService.GetByIdAsync(id);

        return Ok(new FileResponse
        {
            Id = file.Id,
            FileLayoutId = file.FileLayout.Id,
            FileLayoutName = file.FileLayout.Name,
            CompanyId = file.FileLayout.Company.Id,
            CompanyName = file.FileLayout.Company.Name,
            FileName = file.Name,
            ExpectedDate = file.ExpectedDate,
            StatusId = file.FileStatus.Id,
            StatusName = file.FileStatus.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] FileRequest fileRequest)
    {
        var file = new File
        {
            FileLayoutId = fileRequest.FileLayoutId,
            Name = fileRequest.FileName,
            ExpectedDate = fileRequest.ExpectedDate ?? DateTime.Now,
            StatusId = fileRequest.StatusId ?? 1
        };

        var fileCreated = await _fileService.AddAsync(file);
        
        fileCreated = await _fileService.GetByIdAsync(fileCreated.Id);

        await CacheHelper.RemoveCacheAsync(typeof(FileController), _redisDb);

        return Ok(new FileResponse
        {
            Id = fileCreated.Id,
            FileLayoutId = fileCreated.FileLayout.Id,
            FileLayoutName = fileCreated.FileLayout.Name,
            CompanyId = fileCreated.FileLayout.Company.Id,
            CompanyName = fileCreated.FileLayout.Company.Name,
            FileName = fileCreated.Name,
            ExpectedDate = fileCreated.ExpectedDate,
            StatusId = fileCreated.FileStatus.Id,
            StatusName = fileCreated.FileStatus.Name
        });
    }

    [HttpGet("report")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetCountByStatusLastDaysAsync([FromQuery] int lastDays)
    {
        if (lastDays == 0)
            lastDays = _configuration.GetValue<int>("FileConfig:Report:LastDays:FieldValue");

        var result = await _fileService.GetCountByStatusLastDaysAsync(lastDays);
        
        return Ok(result);
    }
}
