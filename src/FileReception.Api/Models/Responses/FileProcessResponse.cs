namespace FileReception.Api.Models.Responses;

public class FileProcessResponse
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePathBackup { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
}
