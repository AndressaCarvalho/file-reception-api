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
public class FileLayoutFieldTypeController : ControllerBase
{
    private readonly IFileLayoutFieldTypeService _fileLayoutFieldTypeService;
    private readonly IDatabase _redisDb;

    public FileLayoutFieldTypeController(IFileLayoutFieldTypeService fileLayoutFieldTypeService, IConnectionMultiplexer redis)
    {
        _fileLayoutFieldTypeService = fileLayoutFieldTypeService;
        _redisDb = redis.GetDatabase();
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var fileLayoutFieldTypes = await _fileLayoutFieldTypeService.GetAllAsync();

        return Ok(fileLayoutFieldTypes.Select(c => new FileLayoutFieldTypeResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var fileLayoutFieldType = await _fileLayoutFieldTypeService.GetByIdAsync(id);

        return Ok(new FileLayoutFieldTypeResponse
        {
            Id = fileLayoutFieldType.Id,
            Name = fileLayoutFieldType.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] FileLayoutFieldTypeRequest fileLayoutFieldTypeRequest)
    {
        var fileLayoutFieldType = new FileLayoutFieldType
        {
            Name = fileLayoutFieldTypeRequest.Name
        };

        var fileLayoutFieldTypeCreated = await _fileLayoutFieldTypeService.AddAsync(fileLayoutFieldType);

        await CacheHelper.RemoveCacheAsync(typeof(FileLayoutFieldTypeController), _redisDb);

        return Ok(new FileLayoutFieldTypeResponse
        {
            Id = fileLayoutFieldTypeCreated.Id,
            Name = fileLayoutFieldTypeCreated.Name
        });
    }
}
