using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Moq;

namespace FileReception.Tests.Service.Services;

public class FileLayoutFieldServiceTests
{
    private readonly Mock<IFileLayoutFieldRepository> _fileLayoutFieldRepositoryMock;
    private readonly FileLayoutFieldService _service;

    public FileLayoutFieldServiceTests()
    {
        _fileLayoutFieldRepositoryMock = new Mock<IFileLayoutFieldRepository>();
        _service = new FileLayoutFieldService(_fileLayoutFieldRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de campos de layout")]
    public async Task GetAllAsync_DeveRetornarListaDeCamposDeLayout()
    {
        // Arrange
        var fields = new List<FileLayoutField>
        {
            new FileLayoutField { Id = 1, Description = "Campo 1", Start = 1, End = 10 },
            new FileLayoutField { Id = 2, Description = "Campo 2", Start = 11, End = 20 }
        };

        _fileLayoutFieldRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fields);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Campo 1", result[0].Description);
        Assert.Equal("Campo 2", result[1].Description);

        _fileLayoutFieldRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar campo de layout quando existir")]
    public async Task GetByIdAsync_DeveRetornarCampoDeLayout_QuandoExistir()
    {
        // Arrange
        var field = new FileLayoutField
        {
            Id = 1,
            Description = "Campo Teste",
            Start = 1,
            End = 10
        };

        _fileLayoutFieldRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(field);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Campo Teste", result.Description);

        _fileLayoutFieldRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando campo de layout não existir")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _fileLayoutFieldRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((FileLayoutField?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Campos delayout de arquivo não encontrado.", exception.Message);

        _fileLayoutFieldRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar campo de layout e retornar a entidade")]
    public async Task AddAsync_DeveAdicionarCampoDeLayout()
    {
        // Arrange
        var field = new FileLayoutField
        {
            Description = "Novo Campo",
            Start = 1,
            End = 5
        };

        _fileLayoutFieldRepositoryMock
            .Setup(r => r.AddAsync(field))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddAsync(field);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Novo Campo", result.Description);

        _fileLayoutFieldRepositoryMock.Verify(r => r.AddAsync(field), Times.Once);
    }
}
