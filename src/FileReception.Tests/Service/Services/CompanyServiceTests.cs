using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Moq;

namespace FileReception.Tests.Service.Services;

public class CompanyServiceTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly CompanyService _service;

    public CompanyServiceTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _service = new CompanyService(_companyRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de companhias")]
    public async Task GetAllAsync_DeveRetornarListaDeCompanhias()
    {
        // Arrange
        var companies = new List<Company>
        {
            new Company { Id = 1, Name = "Empresa A" },
            new Company { Id = 2, Name = "Empresa B" }
        };

        _companyRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(companies);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Empresa A", result[0].Name);
        Assert.Equal("Empresa B", result[1].Name);

        _companyRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar companhia quando existir")]
    public async Task GetByIdAsync_DeveRetornarCompanhia_QuandoExistir()
    {
        // Arrange
        var company = new Company
        {
            Id = 1,
            Name = "Empresa Teste"
        };

        _companyRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(company);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Empresa Teste", result.Name);

        _companyRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando companhia não existir")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Company?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Companhia não encontrada.", exception.Message);

        _companyRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar companhia e retornar a entidade")]
    public async Task AddAsync_DeveAdicionarCompanhia()
    {
        // Arrange
        var company = new Company
        {
            Name = "Nova Empresa"
        };

        _companyRepositoryMock
            .Setup(r => r.AddAsync(company))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddAsync(company);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Nova Empresa", result.Name);

        _companyRepositoryMock.Verify(r => r.AddAsync(company), Times.Once);
    }
}
