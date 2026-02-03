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
public class FileLayoutController : ControllerBase
{
    private readonly IFileLayoutService _fileLayoutService;
    private readonly IDatabase _redisDb;

    public FileLayoutController(IFileLayoutService fileLayoutService, IConnectionMultiplexer redis)
    {
        _fileLayoutService = fileLayoutService;
        _redisDb = redis.GetDatabase();
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var fileLayouts = await _fileLayoutService.GetAllAsync();

        return Ok(fileLayouts.Select(f => new FileLayoutResponse
        {
            Id = f.Id,
            FileLayoutName = f.Name,
            CompanyId = f.Company.Id,
            CompanyName = f.Company.Name
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var fileLayout = await _fileLayoutService.GetByIdAsync(id);

        return Ok(new FileLayoutResponse
        {
            Id = fileLayout.Id,
            FileLayoutName = fileLayout.Name,
            CompanyId = fileLayout.Company.Id,
            CompanyName = fileLayout.Company.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] FileLayoutRequest fileLayoutRequest)
    {
        var fileLayout = new FileLayout
        {
            Name = fileLayoutRequest.Name,
            CompanyId = fileLayoutRequest.CompanyId
        };

        var fileLayoutCreated = await _fileLayoutService.AddAsync(fileLayout);

        fileLayoutCreated = await _fileLayoutService.GetByIdAsync(fileLayoutCreated.Id);

        await CacheHelper.RemoveCacheAsync(typeof(FileLayoutController), _redisDb);

        return Ok(new FileLayoutResponse
        {
            Id = fileLayoutCreated.Id,
            FileLayoutName = fileLayoutCreated.Name,
            CompanyId = fileLayoutCreated.Company.Id,
            CompanyName = fileLayoutCreated.Company.Name
        });
    }
}
