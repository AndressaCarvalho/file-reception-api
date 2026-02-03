using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Moq;

namespace FileReception.Tests.Service.Services;

public class FileStatusServiceTests
{
    private readonly Mock<IFileStatusRepository> _fileStatusRepositoryMock;
    private readonly FileStatusService _service;

    public FileStatusServiceTests()
    {
        _fileStatusRepositoryMock = new Mock<IFileStatusRepository>();
        _service = new FileStatusService(_fileStatusRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de status de arquivo")]
    public async Task GetAllAsync_DeveRetornarListaDeStatus()
    {
        // Arrange
        var status = new List<FileStatus>
        {
            new FileStatus { Id = 1, Name = "Não Recepcionado" },
            new FileStatus { Id = 2, Name = "Recepcionado" }
        };

        _fileStatusRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(status);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Não Recepcionado", result[0].Name);
        Assert.Equal("Recepcionado", result[1].Name);

        _fileStatusRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar status quando existir")]
    public async Task GetByIdAsync_DeveRetornarStatus_QuandoExistir()
    {
        // Arrange
        var status = new FileStatus { Id = 1, Name = "Recepcionado" };

        _fileStatusRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(status);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Recepcionado", result.Name);

        _fileStatusRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando status não existir")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _fileStatusRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((FileStatus?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Status não encontrado.", exception.Message);

        _fileStatusRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar status e retornar a entidade")]
    public async Task AddAsync_DeveAdicionarStatus()
    {
        // Arrange
        var status = new FileStatus { Name = "Erro" };

        _fileStatusRepositoryMock
            .Setup(r => r.AddAsync(status))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddAsync(status);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Erro", result.Name);

        _fileStatusRepositoryMock.Verify(r => r.AddAsync(status), Times.Once);
    }
}
