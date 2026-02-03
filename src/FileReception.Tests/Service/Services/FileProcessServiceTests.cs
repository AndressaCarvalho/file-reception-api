using FileReception.Domain.Entities;
using FileReception.Domain.Interfaces.Repositories;
using FileReception.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using Microsoft.Extensions.Configuration;
using File = FileReception.Domain.Entities.File;

namespace FileReception.Tests.Service.Services;

public class FileProcessServiceTests
{
    private readonly Mock<IFileRepository> _fileRepositoryMock;
    private readonly Mock<IFileProcessRepository> _fileProcessRepositoryMock;
    private readonly Mock<IFileLayoutRepository> _fileLayoutRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<FileService>> _loggerMock;

    private readonly FileProcessService _service;
    private readonly string _backupPath = "C:\\Users\\AndressaCarvalhoArau\\OneDrive - hiperstream.com\\Documentos\\Meu\\Projetos\\FileBackups";

    public FileProcessServiceTests()
    {
        _fileRepositoryMock = new Mock<IFileRepository>();
        _fileProcessRepositoryMock = new Mock<IFileProcessRepository>();
        _fileLayoutRepositoryMock = new Mock<IFileLayoutRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<FileService>>();

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(s => s[It.IsAny<string>()]).Returns(_backupPath);

        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c.GetSection("FileConfig")).Returns(sectionMock.Object);

        _service = new FileProcessService(
            _fileRepositoryMock.Object,
            _fileProcessRepositoryMock.Object,
            _fileLayoutRepositoryMock.Object,
            _configurationMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar lista de processamentos")]
    public async Task GetAllAsync_DeveRetornarLista()
    {
        // Arrange
        var processes = new List<FileProcess>
        {
            new FileProcess { Id = 1, FileId = 1, FilePathBackup = "path1" },
            new FileProcess { Id = 2, FileId = 2, FilePathBackup = "path2" }
        };

        _fileProcessRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(processes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _fileProcessRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar processamento quando existir")]
    public async Task GetByIdAsync_DeveRetornarProcessamento()
    {
        // Arrange
        var process = new FileProcess { Id = 1, FileId = 1 };

        _fileProcessRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(process);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _fileProcessRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve lançar exceção quando não encontrar processamento")]
    public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoExistir()
    {
        // Arrange
        _fileProcessRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((FileProcess?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(99)
        );

        // Assert
        Assert.Equal("Processamento de arquivo não encontrado.", exception.Message);
        _fileProcessRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact(DisplayName = "ProcessFileAsync - Deve lançar exceção quando arquivo nulo")]
    public async Task ProcessFileAsync_DeveLancarExcecao_QuandoArquivoNulo()
    {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ProcessFileAsync(null!)
        );

        // Assert
        Assert.Equal("Arquivo inválido.", exception.Message);
    }

    [Fact(DisplayName = "ProcessFileAsync - Deve lançar exceção quando arquivo não esperado")]
    public async Task ProcessFileAsync_DeveLancarExcecao_QuandoArquivoNaoEsperado()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(10);
        fileMock.Setup(f => f.FileName).Returns("teste.txt");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes("conteudo")));

        _fileRepositoryMock
            .Setup(r => r.GetByNameAndStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((File?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ProcessFileAsync(fileMock.Object)
        );

        // Assert
        Assert.Equal("Arquivo não esperado.", exception.Message);
    }

    [Fact(DisplayName = "ProcessFileAsync - Deve lançar exceção quando layout não encontrado")]
    public async Task ProcessFileAsync_DeveLancarExcecao_QuandoLayoutNaoEncontrado()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(10);
        fileMock.Setup(f => f.FileName).Returns("teste.txt");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes("conteudo")));

        var expectedFile = new File { Id = 1, Name = "teste.txt", FileLayoutId = 1 };
        _fileRepositoryMock
            .Setup(r => r.GetByNameAndStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(expectedFile);

        _fileLayoutRepositoryMock
            .Setup(r => r.GetByIdWithFieldsAsync(It.IsAny<int>()))
            .ReturnsAsync((FileLayout?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ProcessFileAsync(fileMock.Object)
        );

        // Assert
        Assert.Equal("Layout não encontrado.", exception.Message);
    }

    [Fact(DisplayName = "ProcessFileAsync - Deve processar arquivo válido corretamente")]
    public async Task ProcessFileAsync_DeveProcessarArquivoValido()
    {
        // Arrange
        var fileContent = "12345";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(fileContent.Length);
        fileMock.Setup(f => f.FileName).Returns("teste.txt");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)));
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

        var layout = new FileLayout
        {
            Id = 1,
            Fields = new List<FileLayoutField>
            {
                new FileLayoutField
                {
                    Start = 1,
                    End = 5,
                    FileLayoutFieldType = new FileLayoutFieldType { Name = "NUM" },
                    Description = "Campo1"
                }
            }
        };

        var expectedFile = new File { Id = 1, Name = "teste.txt", FileLayoutId = 1 };

        _fileRepositoryMock.Setup(r => r.GetByNameAndStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
                           .ReturnsAsync(expectedFile);

        _fileLayoutRepositoryMock.Setup(r => r.GetByIdWithFieldsAsync(It.IsAny<int>()))
                                 .ReturnsAsync(layout);

        _fileProcessRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileProcess>())).Returns(Task.CompletedTask);
        _fileRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<File>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.ProcessFileAsync(fileMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedFile.Id, result.FileId);
        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);

        _fileProcessRepositoryMock.Verify(r => r.AddAsync(It.IsAny<FileProcess>()), Times.Once);
        _fileRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<File>()), Times.Once);
    }
}
