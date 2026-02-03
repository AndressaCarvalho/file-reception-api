using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Moq;

namespace FileReception.Tests.Service.Services;

public class FileLayoutFieldTypeServiceTests
{
    private readonly Mock<IFileLayoutFieldTypeRepository> _fileLayoutFieldTypeRepositoryMock;
    private readonly FileLayoutFieldTypeService _service;

    public FileLayoutFieldTypeServiceTests()
    {
        _fileLayoutFieldTypeRepositoryMock = new Mock<IFileLayoutFieldTypeRepository>();
        _service = new FileLayoutFieldTypeService(_fileLayoutFieldTypeRepositoryMock.Object);
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de tipos de campo de layout")]
    public async Task GetAllAsync_DeveRetornarListaDeTiposDeCampoDeLayout()
    {
        // Arrange
        var fieldTypes = new List<FileLayoutFieldType>
        {
            new FileLayoutFieldType { Id = 1, Name = "NUM" },
            new FileLayoutFieldType { Id = 2, Name = "TXT" }
        };

        _fileLayoutFieldTypeRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fieldTypes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("NUM", result[0].Name);
        Assert.Equal("TXT", result[1].Name);

        _fileLayoutFieldTypeRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar tipo de campo quando existir")]
    public async Task GetByIdAsync_DeveRetornarTipoDeCampo_QuandoExistir()
    {
        // Arrange
        var fieldType = new FileLayoutFieldType
        {
            Id = 1,
            Name = "NUM"
        };

        _fileLayoutFieldTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(fieldType);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("NUM", result.Name);

        _fileLayoutFieldTypeRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando tipo de campo não existir")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _fileLayoutFieldTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((FileLayoutFieldType?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Tipo de campo de layout não encontrado.", exception.Message);

        _fileLayoutFieldTypeRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar tipo de campo e retornar a entidade")]
    public async Task AddAsync_DeveAdicionarTipoDeCampo()
    {
        // Arrange
        var fieldType = new FileLayoutFieldType
        {
            Name = "TXT"
        };

        _fileLayoutFieldTypeRepositoryMock
            .Setup(r => r.AddAsync(fieldType))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddAsync(fieldType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TXT", result.Name);

        _fileLayoutFieldTypeRepositoryMock.Verify(r => r.AddAsync(fieldType), Times.Once);
    }
}
