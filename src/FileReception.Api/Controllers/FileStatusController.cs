using FileReception.Api.Filters.Cache;
using FileReception.Api.Filters.Cache.Helpers;
using FileReception.Api.Models.Requests;
using FileReception.Api.Models.Responses;
using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FileReception.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileStatusController : ControllerBase
{
    private readonly IFileStatusService _fileStatusService;
    private readonly IDatabase _redisDb;

    public FileStatusController(IFileStatusService fileStatusService, IConnectionMultiplexer redis)
    {
        _fileStatusService = fileStatusService;
        _redisDb = redis.GetDatabase();
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var fileStatus = await _fileStatusService.GetAllAsync();

        return Ok(fileStatus.Select(fs => new FileStatusResponse
        {
            Id = fs.Id,
            Name = fs.Name
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var fileStatus = await _fileStatusService.GetByIdAsync(id);

        return Ok(new FileStatusResponse
        {
            Id = fileStatus.Id,
            Name = fileStatus.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] FileStatusRequest fileStatusRequest)
    {
        var fileStatus = new FileStatus
        {
            Name = fileStatusRequest.Name
        };

        var fileStatusCreated = await _fileStatusService.AddAsync(fileStatus);

        await CacheHelper.RemoveCacheAsync(typeof(FileStatusController), _redisDb);

        return Ok(new FileStatusResponse
        {
            Id = fileStatusCreated.Id,
            Name = fileStatusCreated.Name
        });
    }
}
