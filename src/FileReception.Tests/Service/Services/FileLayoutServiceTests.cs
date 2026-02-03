using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Moq;

namespace FileReception.Tests.Service.Services;

public class FileLayoutServiceTests
{
    private readonly Mock<IFileLayoutRepository> _fileLayoutRepositoryMock;
    private readonly FileLayoutService _service;

    public FileLayoutServiceTests()
    {
        _fileLayoutRepositoryMock = new Mock<IFileLayoutRepository>();
        _service = new FileLayoutService(_fileLayoutRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de layouts de arquivo")]
    public async Task GetAllAsync_DeveRetornarListaDeLayoutsDeArquivo()
    {
        // Arrange
        var layouts = new List<FileLayout>
        {
            new FileLayout { Id = 1, Name = "Layout A", CompanyId = 1 },
            new FileLayout { Id = 2, Name = "Layout B", CompanyId = 2 }
        };

        _fileLayoutRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(layouts);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Layout A", result[0].Name);
        Assert.Equal("Layout B", result[1].Name);

        _fileLayoutRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar layout de arquivo quando existir")]
    public async Task GetByIdAsync_DeveRetornarLayoutDeArquivo_QuandoExistir()
    {
        // Arrange
        var layout = new FileLayout
        {
            Id = 1,
            Name = "Layout Teste",
            CompanyId = 1
        };

        _fileLayoutRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(layout);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Layout Teste", result.Name);

        _fileLayoutRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando layout de arquivo não existir")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _fileLayoutRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((FileLayout?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Layout de arquivo não encontrado.", exception.Message);

        _fileLayoutRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar layout de arquivo e retornar a entidade")]
    public async Task AddAsync_DeveAdicionarLayoutDeArquivo()
    {
        // Arrange
        var layout = new FileLayout
        {
            Name = "Novo Layout",
            CompanyId = 1
        };

        _fileLayoutRepositoryMock
            .Setup(r => r.AddAsync(layout))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddAsync(layout);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Novo Layout", result.Name);
        Assert.Equal(1, result.CompanyId);

        _fileLayoutRepositoryMock.Verify(r => r.AddAsync(layout), Times.Once);
    }
}
