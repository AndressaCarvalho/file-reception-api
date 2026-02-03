using FileReception.Api.Filters.Cache;
using FileReception.Api.Filters.Cache.Helpers;
using FileReception.Api.Models.Requests;
using FileReception.Api.Models.Responses;
using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FileReception.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileLayoutFieldController : ControllerBase
{
    private readonly IFileLayoutFieldService _fileLayoutFieldService;
    private readonly IDatabase _redisDb;

    public FileLayoutFieldController(IFileLayoutFieldService fileLayoutFieldService, IConnectionMultiplexer redis)
    {
        _fileLayoutFieldService = fileLayoutFieldService;
        _redisDb = redis.GetDatabase();
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var fileLayouts = await _fileLayoutFieldService.GetAllAsync();

        return Ok(fileLayouts.Select(f => new FileLayoutFieldResponse
        {
            Id = f.Id,
            FileLayoutId = f.FileLayout.Id,
            FileLayoutName = f.FileLayout.Name,
            Description = f.Description,
            Start = f.Start,
            End = f.End,
            FileLayoutFieldTypeId = f.FileLayoutFieldType.Id,
            FileLayoutFieldTypeName = f.FileLayoutFieldType.Name
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var fileLayoutField = await _fileLayoutFieldService.GetByIdAsync(id);

        return Ok(new FileLayoutFieldResponse
        {
            Id = fileLayoutField.Id,
            FileLayoutId = fileLayoutField.FileLayout.Id,
            FileLayoutName = fileLayoutField.FileLayout.Name,
            Description = fileLayoutField.Description,
            Start = fileLayoutField.Start,
            End = fileLayoutField.End,
            FileLayoutFieldTypeId = fileLayoutField.FileLayoutFieldType.Id,
            FileLayoutFieldTypeName = fileLayoutField.FileLayoutFieldType.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] FileLayoutFieldRequest fileLayoutFieldRequest)
    {
        var fileLayoutField = new FileLayoutField
        {
            FileLayoutId = fileLayoutFieldRequest.FileLayoutId,
            Description = fileLayoutFieldRequest.Description,
            Start = fileLayoutFieldRequest.Start,
            End = fileLayoutFieldRequest.End,
            FileLayoutFieldTypeId = fileLayoutFieldRequest.FileLayoutFieldTypeId
        };

        var fileLayoutFieldCreated = await _fileLayoutFieldService.AddAsync(fileLayoutField);

        await CacheHelper.RemoveCacheAsync(typeof(FileLayoutFieldController), _redisDb);

        fileLayoutFieldCreated = await _fileLayoutFieldService.GetByIdAsync(fileLayoutFieldCreated.Id);

        return Ok(new FileLayoutFieldResponse
        {
            Id = fileLayoutFieldCreated.Id,
            FileLayoutId = fileLayoutFieldCreated.FileLayout.Id,
            FileLayoutName = fileLayoutFieldCreated.FileLayout.Name,
            Description = fileLayoutFieldCreated.Description,
            Start = fileLayoutFieldCreated.Start,
            End = fileLayoutFieldCreated.End,
            FileLayoutFieldTypeId = fileLayoutFieldCreated.FileLayoutFieldType.Id,
            FileLayoutFieldTypeName = fileLayoutFieldCreated.FileLayoutFieldType.Name
        });
    }
}
