namespace FileReception.Domain.Entities;

public class FileProcess : BaseEntity
{
    public int FileId { get; set; }
    public string FilePathBackup { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }

    public virtual File File { get; set; }
}
