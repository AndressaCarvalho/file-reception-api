using FileReception.Domain.Entities;
using FileReception.Service.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace FileReception.Tests.Service.Helpers;

public class FileHelperTests
{
    #region ValidateLines

    [Fact(DisplayName = "ValidateLines - Deve retornar válido quando campos numéricos forem válidos")]
    public void ValidateLines_DeveRetornarValido_QuandoCamposNumericosForemValidos()
    {
        // Arrange
        var layout = CreateNumericLayout();

        var linhas = new[]
        {
            "1234567890",
            "0987654321"
        };

        // Act
        var (isValid, errorMessage) = FileHelper.ValidateLines(linhas, layout);

        // Assert
        Assert.True(isValid);
        Assert.Null(errorMessage);
    }

    [Fact(DisplayName = "ValidateLines - Deve retornar erro quando campos numéricos estiverem vazios")]
    public void ValidateLines_DeveRetornarErro_QuandoCampoNumericoEstiverVazio()
    {
        // Arrange
        var layout = CreateNumericLayout();

        var linhas = new[]
        {
            "          "
        };

        // Act
        var (isValid, errorMessage) = FileHelper.ValidateLines(linhas, layout);

        // Assert
        Assert.False(isValid);
        Assert.Equal("Campo Código é obrigatório.", errorMessage);
    }

    [Fact(DisplayName = "ValidateLines - Deve retornar erro quando campos numéricos não forem números")]
    public void ValidateLines_DeveRetornarErro_QuandoCampoNumericoNaoForNumero()
    {
        // Arrange
        var layout = CreateNumericLayout();

        var lines = new[]
        {
            "12A456"
        };

        // Act
        var (isValid, errorMessage) = FileHelper.ValidateLines(lines, layout);

        // Assert
        Assert.False(isValid);
        Assert.Equal("Campo Código deve ser numérico.", errorMessage);
    }

    #endregion

    #region ReadFileLinesAsync

    [Fact(DisplayName = "ReadFileLines - Deve ler linhas do arquivo")]
    public async Task ReadFileLinesAsync_DeveLerLinhasDoArquivoCorretamente()
    {
        // Arrange
        var content = "linha1\nlinha2\nlinha3";
        var file = CreateIFormFile(content);

        // Act
        var lines = await FileHelper.ReadFileLinesAsync(file);

        // Assert
        Assert.Equal(3, lines.Length);
        Assert.Equal("linha1", lines[0]);
        Assert.Equal("linha2", lines[1]);
        Assert.Equal("linha3", lines[2]);
    }

    #endregion

    #region BackupFileAsync

    [Fact(DisplayName = "BackupFile - Deve salvar arquivo no diretório")]
    public async Task BackupFileAsync_DeveSalvarArquivoNoDiretorio()
    {
        // Arrange
        var content = "conteudo do arquivo";
        var file = CreateIFormFile(content, "teste.txt");

        var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var loggerMock = new Mock<ILogger>();

        // Act
        var path = await FileHelper.BackupFileAsync(file, tempFolder, loggerMock.Object);

        // Assert
        Assert.True(System.IO.File.Exists(path));

        var outputText = await System.IO.File.ReadAllTextAsync(path);
        
        Assert.Equal(content, outputText);
    }

    #endregion

    #region Helpers

    private static FileLayout CreateNumericLayout()
    {
        return new FileLayout
        {
            Fields = new List<FileLayoutField>
            {
                new FileLayoutField
                {
                    Description = "Código",
                    Start = 1,
                    End = 6,
                    FileLayoutFieldType = new FileLayoutFieldType
                    {
                        Name = "NUM"
                    }
                }
            }
        };
    }

    private static IFormFile CreateIFormFile(string content, string fileName = "arquivo.txt")
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        return new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
    }

    #endregion
}
