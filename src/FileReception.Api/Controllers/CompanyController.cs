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
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IDatabase _redisDb;

    public CompanyController(ICompanyService companyService, IConnectionMultiplexer redis)
    {
        _companyService = companyService;
        _redisDb = redis.GetDatabase();
    }

    [HttpGet]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetAllAsync()
    {
        var companies = await _companyService.GetAllAsync();

        return Ok(companies.Select(c => new CompanyResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToList());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(CacheFilter))]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var company = await _companyService.GetByIdAsync(id);

        return Ok(new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CompanyRequest companyRequest)
    {
        var company = new Company
        {
            Name = companyRequest.Name
        };

        var companyCreated = await _companyService.AddAsync(company);

        await CacheHelper.RemoveCacheAsync(typeof(CompanyController), _redisDb);

        return Ok(new CompanyResponse
        {
            Id = companyCreated.Id,
            Name = companyCreated.Name
        });
    }
}
