using FileReception.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FileReception.Service.Helpers;

public static class FileHelper
{
    public static async Task<string[]> ReadFileLinesAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var content = await reader.ReadToEndAsync();

        return content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static (bool isValid, string? errorMessage) ValidateLines(string[] lines, FileLayout layout)
    {
        foreach (var line in lines)
        {
            if (!ValidateLine(line, layout, out string? error))
                return (false, error);
        }

        return (true, null);
    }

    private static bool ValidateLine(string line, FileLayout layout, out string? errorMessage)
    {
        foreach (var field in layout.Fields.OrderBy(f => f.Start))
        {
            var value = ReadField(line, field.Start, field.End);

            if (field.FileLayoutFieldType.Name == "NUM")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    errorMessage = $"Campo {field.Description} é obrigatório.";
                    
                    return false;
                }

                if (!value.All(char.IsDigit))
                {
                    errorMessage = $"Campo {field.Description} deve ser numérico.";
                    
                    return false;
                }
            }
        }

        errorMessage = null;

        return true;
    }

    public static async Task<string> BackupFileAsync(IFormFile file, string backupPath, ILogger logger)
    {
        Directory.CreateDirectory(backupPath);
        var backupFileName = Path.Combine(backupPath, file.FileName);

        await using var fileStream = new FileStream(backupFileName, FileMode.Create);
        await file.CopyToAsync(fileStream);

        logger.LogInformation("Arquivo salvo em backup no caminho {BackupPath},", backupFileName);

        return backupFileName;
    }

    private static string ReadField(string line, int start, int end)
    {
        var startIndex = start - 1;

        if (line.Length <= startIndex)
            return string.Empty;

        var maxLength = end - start + 1;
        var availableLength = line.Length - startIndex;

        var length = Math.Min(maxLength, availableLength);

        return line.Substring(startIndex, length).Trim();
    }
}
