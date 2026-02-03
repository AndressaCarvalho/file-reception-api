using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Moq;
using File = FileReception.Domain.Entities.File;

namespace FileReception.Tests.Service.Services;

public class FileServiceTests
{
    private readonly Mock<IFileRepository> _fileRepositoryMock;
    private readonly FileService _service;

    public FileServiceTests()
    {
        _fileRepositoryMock = new Mock<IFileRepository>();
        _service = new FileService(_fileRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de arquivos")]
    public async Task GetAllAsync_DeveRetornarListaDeArquivos()
    {
        // Arrange
        var files = new List<File>
        {
            new File { Id = 1, Name = "arquivo1.txt" },
            new File { Id = 2, Name = "arquivo2.txt" }
        };

        _fileRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(files);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("arquivo1.txt", result[0].Name);
        Assert.Equal("arquivo2.txt", result[1].Name);

        _fileRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar arquivo quando existir")]
    public async Task GetByIdAsync_DeveRetornarArquivo_QuandoExistir()
    {
        // Arrange
        var file = new File
        {
            Id = 1,
            Name = "arquivo.txt"
        };

        _fileRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(file);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("arquivo.txt", result.Name);

        _fileRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando arquivo não existir")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _fileRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((File?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Arquivo não encontrado.", exception.Message);

        _fileRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "GetCountByStatusLastDaysAsync - Deve retornar contagem por status")]
    public async Task GetCountByStatusLastDaysAsync_DeveRetornarContagemPorStatus()
    {
        // Arrange
        var lastDays = 7;
        var fromDate = DateTime.Now.AddDays(-lastDays);

        var expectedResult = new Dictionary<string, int>
        {
            { "Recepcionado", 5 },
            { "Erro", 1 }
        };

        _fileRepositoryMock
            .Setup(r => r.GetCountByStatusLastDaysAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetCountByStatusLastDaysAsync(lastDays);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(5, result["Recepcionado"]);
        Assert.Equal(1, result["Erro"]);

        _fileRepositoryMock.Verify(
            r => r.GetCountByStatusLastDaysAsync(It.IsAny<DateTime>()),
            Times.Once
        );
    }

    [Fact(DisplayName = "GetCountByStatusLastDaysAsync - Deve lançar exceção quando dias inválidos")]
    public async Task GetCountByStatusLastDaysAsync_DeveLancarExcecao_QuandoDiasInvalidos()
    {
        // Arrange
        var invalidDays = 0;

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetCountByStatusLastDaysAsync(invalidDays)
        );

        // Assert
        Assert.Equal("Informe um número de dias válido.", exception.Message);

        _fileRepositoryMock.Verify(
            r => r.GetCountByStatusLastDaysAsync(It.IsAny<DateTime>()),
            Times.Never
        );
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar arquivo e retornar a entidade")]
    public async Task AddAsync_DeveAdicionarArquivo()
    {
        // Arrange
        var file = new File
        {
            Name = "novo_arquivo.txt"
        };

        _fileRepositoryMock
            .Setup(r => r.AddAsync(file))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddAsync(file);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("novo_arquivo.txt", result.Name);

        _fileRepositoryMock.Verify(r => r.AddAsync(file), Times.Once);
    }
}
